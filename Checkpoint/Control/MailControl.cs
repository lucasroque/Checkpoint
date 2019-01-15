using Checkpoint.Model;
using System;
using System.Net.Mail;

namespace Checkpoint.Control
{
    class MailControl
    {
        private  MailManager mailManager;

        public MailControl(MailManager mailManager)
        {
            this.mailManager = mailManager;
        }

        public SmtpClient getSmtpClient()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = mailManager.host;
            smtp.Port = mailManager.port;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(mailManager.user, mailManager.password);

            return smtp;
        }

        public async void sendMail(Email email)
        {
            SmtpClient smtpClient = getSmtpClient();

            try
            {
                using (MailMessage mail = new MailMessage())
                {

                    mail.From = new MailAddress(mailManager.user);
                    mail.To.Add(new MailAddress(mailManager.user));
                    mail.Subject = email.subject;
                    mail.Body = email.content;

                    foreach (String file in email.attachments)
                    {
                        mail.Attachments.Add(new Attachment(file));
                    }

                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                smtpClient.Dispose();
            }
        }
    }
}
