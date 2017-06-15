using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Helper
{
    
    public class TransactionHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ICollection<Transaction> TransactionAccount()
        {
            Account account = new Account();
            var trans = db.Transactions.Where(u => u.AccountId == account.Id).ToList();
            return (trans);
        }      

        public ICollection<Transaction> UserTransactions()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var trans = db.Transactions.Where(u => u.Account.HouseHoldId == user.HouseHoldId && u.Date.Month == DateTimeOffset.Now.Month).ToList();
            return (trans);
        }


        public double Total()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var category = db.Categories.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            foreach (var item in category)
            {
                var transact = db.Transactions.Where(u => u.CategoryId == item.Id).ToList();
                var group = (from w in transact
                             group w by w.Category.Name into n
                             select n).Distinct();                              
            }
            
            
            var myaccount = db.Accounts.First(u => u.HouseHoldId == user.HouseHoldId);
            var trans = db.Transactions.Where(u => u.Account.HouseHoldId == user.HouseHoldId).ToList();
            var groups = (from w in trans.Where(u => u.Void == false && u.TransactionTypeId != 1)
                          group w by w.Category.Name into n
                          select n).Distinct().ToList();
            

            foreach (var cat in category)
            {
                var bob = db.Transactions.Where(u => u.CategoryId == cat.Id).Select(u => u.Amount).Sum();
                return (bob);
            }
            return (23);
        }
    }
}