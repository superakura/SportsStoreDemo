using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;
using System.Net;
using System.Net.Mail;

namespace SportsStoreDomain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "superakura@hotmail.com";
        public string MailFromAddress = "gaoyuan-dl@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "Smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"d:\";
    }
    public class EmailOrderProcessor:IOrderProcessor
    {
        private EmailSettings _emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            this._emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetail shippingInfo)
        {
            using (var smtpClient=new SmtpClient())
            {
                smtpClient.EnableSsl = this._emailSettings.UseSsl;
                smtpClient.Host = this._emailSettings.ServerName;
                smtpClient.Port = this._emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(this._emailSettings.Username, this._emailSettings.Password);
                if (this._emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = this._emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("一个新的订单已经被提交")
                    .AppendLine("---")
                    .AppendLine("项目：");
                foreach (var line in cart.Lines)
                {
                    var subTotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0}*{1}(小计：{2:c}",
                        line.Quantity,
                        line.Product.Name,
                        subTotal);
                }
                body.AppendFormat("订单总价：{0:c}", cart.ComputerTotalValue())
                    .AppendLine("---")
                    .AppendLine("邮寄到：")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2??"")
                    .AppendLine(shippingInfo.Line3??"")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State??"")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("是否礼物包装:{0}",shippingInfo.GiftWrap?"是":"否");
                MailMessage mailMessage = new MailMessage(
                    this._emailSettings.MailFromAddress,
                    this._emailSettings.MailToAddress,
                    "一个新订单已生成！",
                    body.ToString());
                if (this._emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
