﻿using AFModels;
using BDS.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BDS.DAO
{
    public class LienHeDAO
    {
        BatDongSanContext db = new BatDongSanContext();
        public List<LienHeViewModel> list()
        {
            List<LienHeViewModel> kq = (from lh in db.LienHes
                                          select new LienHeViewModel
                                          {
                                              MaLienHe = lh.MaLienHe,
                                              HoTen = lh.HoTen,
                                              Email = lh.Email,
                                              TieuDe = lh.TieuDe,
                                              LoiNhan = lh.LoiNhan
        

                                          }).ToList();
            return kq;
        }


        public IEnumerable<LienHe> ListAll(int pageNumber, int pageSize)
        {
            return db.LienHes.OrderByDescending(s => s.MaLienHe).ToPagedList(pageNumber, pageSize);
        }
        public long Insert(LienHe lh)
        {
            db.LienHes.Add(lh);
            db.SaveChanges();
            return lh.MaLienHe;
        }

        public LienHe GetByID(int id)
        {
            return db.LienHes.Find(id);
        }
        public IEnumerable<LienHe> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<LienHe> model = db.LienHes;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TieuDe.Contains(searchString) || x.TieuDe.Contains(searchString));
            }

            return model.OrderByDescending(x => x.MaLienHe).ToPagedList(page, pageSize);
        }


        //public void SendEmail(string address, string subject, string message)
        //{
        //    string email = "buiminhquy2105@gmail.com";

        //    string password = "0332813815Aa";


        //    var msg = new MailMessage();
        //    var smtpClient = new SmtpClient("smtp.gmail.com", 587);
        //    smtpClient.EnableSsl = true;
        //    var loginInfo = new NetworkCredential(email, password);

        //    msg.From = new MailAddress(email);
        //    msg.To.Add(new MailAddress(address));
        //    msg.Subject = subject;
        //    msg.Body = message;
        //    msg.IsBodyHtml = true;


        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.Credentials = loginInfo;
        //    smtpClient.Send(msg);
        //}






        public void SendEmail1(string address, string subject, string message)
        {
            string email = "buiminhquy2105@gmail.com";
            string password = "0332813815Aa";


            var loginInfo = new NetworkCredential(email, "ppxyrfriqsavtngq");
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            //smtpClient.Send(msg);
        }


        public void SendMail(string toEmailAddress, string subject, string content)
        {

            // Để Gmail cho phép SmtpClient kết nối đến server SMTP của nó với xác thực 
            //là tài khoản gmail của bạn, bạn cần thiết lập tài khoản email của bạn như sau:
            //Vào địa chỉ https://myaccount.google.com/security  Ở menu trái chọn mục Bảo mật, sau đó tại mục Quyền truy cập 
            //của ứng dụng kém an toàn phải ở chế độ bật
            //  Đồng thời tài khoản Gmail cũng cần bật IMAP
            //Truy cập địa chỉ https://mail.google.com/mail/#settings/fwdandpop

            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"].ToString());

            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }

        //public static bool sendemail (string to,string subject, string body)
        //{
        //    try
        //    {
        //        var msg = new system.net.mail.mailmessage();
        //        msg.to.add(to);
        //        msg.subject = subject;
        //        msg.body = body;
        //        if (msg.body != null)
        //        {
        //            msg.isbodyhtml = msg.body.contains("</");
        //        }
        //        var smtp = new system.net.mail.smtpclient();
        //        smtp.enablessl = true;
        //        smtp.send(msg);
        //    }
        //    catch (exception ex)
        //    {

        //        return false;
        //    }
        //    return true;
        //}

        public void Reply(string toEmailAddress, string subject, string content)
        {

            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"].ToString());

            string body = content;

            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enableSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }
    }
}