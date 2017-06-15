using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Helper
{
    public class NotificationsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<Notification> ListNotifications(string userId)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var notify = db.Notifications.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();

            return (notify);    
        }

        public ICollection<Account> ListAccount()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            List<Account> zero = new List<Account>();
            foreach (var item in account)
            {
                if (item.Balance < 100 && item.Balance > 0)
                {
                    zero.Add(item);
                    return (zero);
                }
            }
            return (account);
        }
    }
}