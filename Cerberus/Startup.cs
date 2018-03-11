using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace Cerberus
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
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("AppDatabase"));
            });
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
                options.Password.RequiredUniqueChars = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication()
                .AddJwtBearer(cfg =>
                {
#if DEBUG
                    cfg.RequireHttpsMetadata = false;
#else
                    cfg.RequireHttpsMetadata = true;
#endif
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


            services.Configure<MvcOptions>(options =>
            {
                #if !DEBUG
                options.Filters.Add(new RequireHttpsAttribute());
                #endif
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
                    new CultureInfo("en-US"),
                    new CultureInfo("ru-RU")
                };  
                options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            }); 
            
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = Configuration["ReCaptcha:SiteKey"],
                SecretKey = Configuration["ReCaptcha:SecretKey"]
            });

            services.AddScoped<AuthService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationContext context, IServiceProvider serviceProvider)
        {
            var localizationOption = serviceProvider.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOption.Value);
            
            CreateRolesAsync(serviceProvider).Wait();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

#if !DEBUG
            var rewriterOptions = new RewriteOptions()
                .AddRedirectToHttps();
            app.UseRewriter(rewriterOptions);
#endif

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "admin", "user" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (roleExist)
                    continue;

                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (!await userManager.Users.AnyAsync())
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = Configuration["DefaultUser:Email"],
                    DisplayName = Configuration["DefaultUser:DisplayName"],
                    Email = Configuration["DefaultUser:Email"]
                };
                var createPowerUser = await userManager.CreateAsync(defaultUser, Configuration["DefaultUser:Password"]);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "admin");
                    await userManager.AddToRoleAsync(defaultUser, "user");
                }
            }
        }
    }
}
