using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Global
{
    public class SendMail
    {
        public static bool Send(string To, string Subject, string Body)
        {
            string from = ConfigurationManager.AppSettings["Mail_From"].ToString();
            string display_name = ConfigurationManager.AppSettings["Mail_From_Display"].ToString();
            return Send(To, from, display_name, Subject, Body);
        }

        public static bool Send(string To, string From, string FromName, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(From, FromName);
            mail.To.Add(To);
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Subject = Subject;
            mail.IsBodyHtml = true; //是否允许内容为 HTML 格式
            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["Mail_Host"]);
            smtp.UseDefaultCredentials = false; // 是否使用本地验证
            System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential();
            SMTPUserInfo.UserName = ConfigurationManager.AppSettings["Mail_Username"]; // 用户名
            SMTPUserInfo.Password = ConfigurationManager.AppSettings["Mail_Password"]; // 登录密码
            smtp.Credentials = SMTPUserInfo; // 验证信息
            smtp.EnableSsl = false;

            try
            {
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }            
        }
    }
}
