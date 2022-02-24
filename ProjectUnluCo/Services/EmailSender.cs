﻿using Microsoft.Extensions.Options;
using ProjectUnluCo.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUnluCo.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SMTPConfigModel _config;
       
      
        public EmailSender(IOptions<SMTPConfigModel> options)
        {
            this._config = options.Value;
           
        }
       
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Execute(email, subject, message);
        }

        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                
                MailMessage mail = new MailMessage
                {
                    Subject = subject,
                    Body = message,
                    From = new MailAddress(_config.SenderAddress, _config.SenderDisplayName),
                    IsBodyHtml = _config.IsBodyHtml
                };
                mail.To.Add(email);

                
                NetworkCredential networkCredentials = new NetworkCredential(_config.Username, _config.Password);

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _config.Host,
                    Port = _config.Port,
                    EnableSsl = _config.EnableSSL,
                    Credentials = networkCredentials
                };

                mail.BodyEncoding = Encoding.Default;
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message.ToString();
                
            }
        }
    }
}
