using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace login_v6.Models
{
    /// <summary>
    /// Envío de correos por SMTP. Los datos del servidor y la cuenta se leen de appSettings
    /// (claves "Smtp:*"), que viven en appSettings.secret.config (no versionado).
    /// </summary>
    public class MailSender
    {
        public async Task<bool> sendMail(string toEmailAddress, string subject, string htmlMessage)
        {
            var host = ConfigurationManager.AppSettings["Smtp:Host"];
            var usuario = ConfigurationManager.AppSettings["Smtp:Usuario"];
            var password = ConfigurationManager.AppSettings["Smtp:Password"];
            var remitente = ConfigurationManager.AppSettings["Smtp:Remitente"] ?? usuario;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                Trace.TraceError("SMTP sin configurar: complete las claves Smtp:* en appSettings.secret.config.");
                return false;
            }

            int puerto;
            if (!int.TryParse(ConfigurationManager.AppSettings["Smtp:Puerto"], out puerto))
            {
                puerto = 587;
            }

            using (var mail = new MailMessage())
            using (var smtp = new SmtpClient(host, puerto))
            {
                mail.From = new MailAddress(remitente);
                mail.To.Add(toEmailAddress);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = htmlMessage;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(usuario, password);
                smtp.EnableSsl = true;

                try
                {
                    await smtp.SendMailAsync(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error al enviar correo a {0}: {1}", toEmailAddress, ex);
                    return false;
                }
            }
        }
    }
}
