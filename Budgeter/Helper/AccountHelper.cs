using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Helper
{
    public class AccountHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public double ShowAccount()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var account = db.Accounts.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            double total = 0;
            foreach (var item in account)
            {
                total += item.Balance;
            }
            return total;
        }

        public double ShowChecking()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var account = db.Accounts.FirstOrDefault(u => u.HouseHoldId == user.HouseHoldId && u.Name == "Checking");
            return account.Balance;
        }

        public double TransCredit()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var myaccount = db.Accounts.First(u => u.HouseHoldId == user.HouseHoldId);
            var trans = db.Transactions.Where(u => u.Account.HouseHoldId == user.HouseHoldId).Where(z => z.Date.Month == DateTimeOffset.Now.Month).ToList();
            var total1 = trans.Where(u => u.TransactionTypeId == 1 && u.Void == false).ToList();
            double total = 0;
    
            foreach (var item in total1)
            {
                total += item.Amount;
            }
            return (total);
        }

        public double TransDebit()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var myaccount = db.Accounts.First(u => u.HouseHoldId == user.HouseHoldId);
            var trans = db.Transactions.Where(u => u.Account.HouseHoldId == user.HouseHoldId).Where(z => z.Date.Month == DateTimeOffset.Now.Month).ToList();
            var total1 = trans.Where(u => u.TransactionTypeId == 2 && u.Void == false).ToList();
            double total = 0;

            foreach (var item in total1)
            {
                total += item.Amount;
            }
            return (total);
        }
    }
}