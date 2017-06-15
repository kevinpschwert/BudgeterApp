using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BudgetApp.Helper
{
    public class HouseholdHelper 
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<ApplicationUser> UserInHousehold(int householdId)
        {
            return db.HouseHolds.Find(householdId).User;
        }

       

       
        public async Task SendEmailRequest(Invitation invite, ApplicationUser user, string sendemail)
        {
            IdentityMessage message = new IdentityMessage();
            Guid id = Guid.NewGuid();
            var guid = id.ToString();
            var houseId = db.HouseHolds.FirstOrDefault(u => u.Id == user.HouseHoldId).Id;
            invite.HouseHoldId = houseId;
            invite.Email = sendemail;
            invite.Generated = DateTimeOffset.Now;
            invite.Expiration = DateTime.Today.AddDays(7);
            invite.Code = guid;
            db.Invitations.Add(invite);
            db.SaveChanges();
            var sendto = sendemail;            
            message.Destination = sendto;
            var useremail = user.Email;
            //var callbackUrl = Url.Action("Bob", "Home", new { id = houseId });
            //var callbackUrl = Url.Action("Index", "Home", new { id = houseId }, protocol: Request.Url.Scheme);
            string callbackUrl = "http://kschwert-budgeter.azurewebsites.net/Home/LogMeInJoin?code=" + guid;
            message.Subject = "You have been invited!";
            message.Body = "Hello! You have been invited by " + user.FirstName + " to join the HouseHold " + invite.HouseHold.Name + " Budget Group! All you have to do is click <a href=\"" + callbackUrl + "\">here</a>. Your code is " + guid;

            //var house = new HouseHold();

            var email =
                new MailMessage(new MailAddress(useremail),
                 new MailAddress(sendto))
                {
                    Subject = message.Subject,
                    //Body = "You have been invited by " + user.FirstName + " to join the HouseHold " + houseName + " budget group! Click here</a>",
                    Body = message.Body,
                    IsBodyHtml = true
                };
            var svc = new PersonalEmail();

            //return RedirectToAction("Sent");
            var GmailUsername = WebConfigurationManager.AppSettings["username"];
            var GmailPassword = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            })

                try
                {
                    await svc.SendAsync(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
        }
    }
}

