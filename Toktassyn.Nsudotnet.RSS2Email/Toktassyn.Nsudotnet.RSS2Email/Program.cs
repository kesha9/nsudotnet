using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.ServiceModel.Syndication;
using System.Net.Mail;
using System.Net;
using System.Xml;


namespace Toktassyn.Nsudotnet.RSS2Email
{
    class RSS2Email
    {
        public int Port { get; set; }
        public int Size { get; set; }
        public string Login { get; set; }
        public string Url { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string smAddress { get; set; }


        public RSS2Email() { }    

        private void InitDates (object obj, ElapsedEventArgs elapsedEventArgs)
        {
            var Reader = XmlReader.Create(Url);
            var item = SyndicationFeed.Load(Reader);
           
            
            if (null != item)
            {
                Send(item);
            }

        }

        public void Begin()
        {
            var _time = new Timer(Size);
            _time.Elapsed += InitDates;
            _time.Enabled = true;
            Console.ReadLine();
        }

        private void Send(SyndicationFeed item)
        {
            foreach (var _item in item.Items)
            {
             var _smtp = new SmtpClient(smAddress, Port);
             _smtp.Credentials = new NetworkCredential(Login, Password);
             var _mess = new MailMessage();
             _mess.From = new MailAddress(From);
             _mess.To.Add(new MailAddress(To));
             _mess.Subject = _item.Title.Text;
             _mess.Body = _item.Summary.Text;

             Console.WriteLine(_mess.Body);
             Console.WriteLine();
             _smtp.Send(_mess);
            }
        }

    }


    class Program
    {

        static void Main(string[] args)
        {
            var rssmail = new RSS2Email
            {
                
                Url = "https://news.mail.ru//rss.xml",
                Size = 1000,
                Port = 587, 
                
                smAddress = "smtp.mail.ru",
                Login = "erkesh_94_",
                Password = "",
                
                From = "erkesh_94_@mail.ru",
                To = "akerke.toktassyn@gmail.com", 
            };
            rssmail.Begin();
        }
       }
}
