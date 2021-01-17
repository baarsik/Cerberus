using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Services.Interfaces;

namespace Web.Controllers.Services
{
    public class EmailService : BaseService
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _fromMailAddress;
        
        public EmailService(IDbContextFactory<ApplicationContext> dbContextFactory, UserManager<ApplicationUser> userManager, IConfiguration configuration,
            IViewRenderService viewRenderService)
            : base(dbContextFactory, userManager, configuration)
        {
            _viewRenderService = viewRenderService;

            var configSection = Configuration.GetSection("Smtp");
            _fromMailAddress = new MailAddress(configSection["Username"]);
            _smtpClient = new SmtpClient(configSection["Host"])
            {
                Credentials = new NetworkCredential(configSection["Username"], configSection["Password"])
            };
        }

        public async Task SendEmailAsync(string recipient, string subject, string viewName, object viewModel)
        {
            await SendEmailAsync(new[] {recipient}, subject, viewName, viewModel);
        }
        
        public async Task SendEmailAsync(IEnumerable<string> recipients, string subject, string viewName, object viewModel)
        {
            var message = new MailMessage()
            {
                From = _fromMailAddress,
                Subject = subject,
                Body = await _viewRenderService.RenderToStringAsync(viewName, viewModel)
            };
            foreach (var recipient in recipients)
            {
                message.To.Add(recipient);
            }
            
            await _smtpClient.SendMailAsync(message);
        }
    }
}