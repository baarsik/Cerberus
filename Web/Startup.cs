﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Controllers.Services;
using Web.Models.Helpers;
using DataContext;
using DataContext.Models;
using DataContextDataFiller;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using reCAPTCHA.AspNetCore;
using Web.Services.Implementations;
using Web.Services.Interfaces;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextFactory<ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppDatabase"));
            });
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IDbContextFactory<ApplicationContext>>().CreateDbContext());
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 4;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
                options.User.RequireUniqueEmail = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtIssuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
            
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };  
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider()
                };
            }); 
            
            services.Configure<RecaptchaSettings>(Configuration.GetSection("RecaptchaSettings"));
            services.AddTransient<IRecaptchaService, RecaptchaService>();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            
            services.AddSingleton(Configuration);
            
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<DataHelper>();
            
            var autoRegisterBaseClasses = new List<Type> { typeof(BaseService) };
            var autoRegisterClasses = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsClass && autoRegisterBaseClasses.Contains(x.BaseType) && !x.IsAbstract)
                .ToList();
            autoRegisterClasses.ForEach(x => services.AddScoped(x));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context, IServiceProvider serviceProvider)
        {
            var localizationOption = serviceProvider.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOption.Value);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseCors();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=WebNovel}/{action=Index}");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            
            app.UseStatusCodePages();
            app.UseStatusCodePagesWithRedirects("/error/{0}.html");
            
            context.Database.Migrate();

            CreateRolesAsync(serviceProvider).Wait();
            InsertData(serviceProvider);
        }
        
        private async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            var roleNames = typeof(Constants.Roles)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(c => c.GetValue(null) as string)
                .ToList();
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (roleExist)
                    continue;

                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (!await userManager.Users.AnyAsync(c => c.Email == Configuration["DefaultUser:Email"]))
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = Configuration["DefaultUser:Email"],
                    DisplayName = Configuration["DefaultUser:DisplayName"],
                    Email = Configuration["DefaultUser:Email"],
                    ApiKey = StringHelpers.GenerateRandomString(64)
                };
                var createPowerUser = await userManager.CreateAsync(defaultUser, Configuration["DefaultUser:Password"]);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddClaimAsync(defaultUser, new Claim("DisplayName", defaultUser.DisplayName));
                    await userManager.AddToRoleAsync(defaultUser, Constants.Roles.Admin);
                    await userManager.AddToRoleAsync(defaultUser, Constants.Roles.User);
                }
            }
        }

        private void InsertData(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationContext>();
            var dataInserter = new DataInserter();
            dataInserter.InsertValues(dbContext);
            dbContext.SaveChanges();
        }
    }
}
