using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
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
            return await Enviar(toEmailAddress, subject, htmlMessage) == null;
        }

        /// <summary>
        /// Envía el mail. Devuelve null si salió, o el motivo por el que no se pudo enviar (también queda en el Trace).
        /// Nunca lanza excepciones. textoPlano (opcional) es la versión sin formato para clientes de correo sin HTML.
        /// </summary>
        public async Task<string> Enviar(string toEmailAddress, string subject, string htmlMessage, string textoPlano = null)
        {
            var host = ConfigurationManager.AppSettings["Smtp:Host"];
            var usuario = ConfigurationManager.AppSettings["Smtp:Usuario"];
            var password = ConfigurationManager.AppSettings["Smtp:Password"];
            var remitente = ConfigurationManager.AppSettings["Smtp:Remitente"] ?? usuario;
            var nombreRemitente = ConfigurationManager.AppSettings["Smtp:NombreRemitente"] ?? "SGEI";

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                const string sinConfigurar = "SMTP sin configurar: complete las claves Smtp:* en appSettings.secret.config.";
                Trace.TraceError(sinConfigurar);
                return sinConfigurar;
            }

            int puerto;
            if (!int.TryParse(ConfigurationManager.AppSettings["Smtp:Puerto"], out puerto))
            {
                puerto = 587;
            }

            try
            {
                using (var mail = new MailMessage())
                using (var smtp = new SmtpClient(host, puerto))
                {
                    mail.From = new MailAddress(remitente, nombreRemitente, Encoding.UTF8);
                    mail.To.Add(toEmailAddress);
                    mail.Subject = subject;
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;
                    if (string.IsNullOrEmpty(textoPlano))
                    {
                        mail.IsBodyHtml = true;
                        mail.Body = htmlMessage;
                    }
                    else
                    {
                        mail.Body = textoPlano;
                        mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlMessage, Encoding.UTF8, MediaTypeNames.Text.Html));
                    }

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(usuario, password);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 30000;

                    await smtp.SendMailAsync(mail);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error al enviar correo a {0}: {1}", toEmailAddress, ex);
                var interna = ex.InnerException != null ? " (" + ex.InnerException.Message + ")" : "";
                return ex.Message + interna;
            }
        }
    }
}
