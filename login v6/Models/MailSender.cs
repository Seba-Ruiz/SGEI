using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace login_v6.Models
{
    public class MailSender
    {

        public async Task<bool> sendMail(string toEmailAddress, string subject, string htmlMessage)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("antivirushospi@gmail.com");
            mail.To.Add(toEmailAddress);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlMessage;
            SmtpServer.Port = 25;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("antivirushospi@gmail.com", "hospital2015");
            SmtpServer.EnableSsl = true;

            try
            {
                await Task.Run(() => SmtpServer.Send(mail));
                return true;
            }
            catch (Exception) { return false; }
        }



    }


    




}