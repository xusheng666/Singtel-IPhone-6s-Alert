using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WebInfoCrawler
{
    public class EmailHelper
    {
        public void sendEmail(EmailObject email)
        {
            MailMessage mail = new MailMessage("admin@webguru.com", "kikky666@163.com");
            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "192.168.1.17";
            mail.Subject = email.MailSubject;
            mail.Body = email.MailBody;
            client.Send(mail);
        }

        public void sendAlertEmail(List<IPhoneStatus> iphoneList)
        {
            if (iphoneList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Dear Mdm/ Sir, \r\n");
                sb.Append("Good News, below are the available Sintel Shop to sign: \r\n ");
                foreach (var item in iphoneList)
                {
                    sb.Append(item.branch + "\r\n");
                }

                EmailObject email = new EmailObject();
                email.MailBody = sb.ToString();
                email.MailSubject = "64GB IPhone 6s Rosegold Avaliable for below Singtel Shop";

                sendEmail(email);
            }
        }
    }

    public class EmailObject
    {
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string mailFrom { get; set; }
        public string mailTo { get; set; }
    }
}
