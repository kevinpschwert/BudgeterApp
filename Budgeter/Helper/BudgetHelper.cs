using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Helper
{
    public class BudgetHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void ManageAccount(Transaction transaction, string datepicker, string Description, string Amount)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            transaction.Date = Convert.ToDateTime(datepicker);
            transaction.Description = Description;
            transaction.Amount = Convert.ToDouble(Amount);
            transaction.ReconciledAmount = 0;
            transaction.Void = false;
            transaction.Reconciled = false;
            transaction.EnteredById = user.Id;
            db.Transactions.Add(transaction);
            db.SaveChanges();            
            Category category = db.Categories.FirstOrDefault(u => u.Id == transaction.CategoryId);
            Budget budget = db.Budgets.FirstOrDefault(u => u.Id == category.BudgetId);
            Account account = db.Accounts.First(u => u.Id == transaction.AccountId);            
            if (transaction.TransactionTypeId == 2)
            {
                account.Balance -= transaction.Amount;
                db.SaveChanges();

            }
            if (transaction.TransactionTypeId == 1)
            {
                account.Balance += transaction.Amount;
                db.SaveChanges();
            }
            if (budget != null)
            {
                budget.TransactionAmount += transaction.Amount;
                db.SaveChanges();
            }
            
            Notification notify = new Notification();
            if (account.Balance > 0 && account.Balance < 100)
            {
                notify.Created = DateTimeOffset.Now;
                notify.Description = "Caution: Your " + account.Name + " account has less than $100.00 in it.";
                notify.HouseHoldId = user.HouseHoldId ?? 1;
                db.Notifications.Add(notify);
                db.SaveChanges();
            }
            if (account.Balance <= 0)
            {
                notify.Created = DateTimeOffset.Now;
                notify.Description = "Warning: Your " + account.Name + " account has less than $0 in it.";
                notify.HouseHoldId = user.HouseHoldId ?? 1;
                db.Notifications.Add(notify);
                db.SaveChanges();
            }
        }

        public void Void(Transaction transaction)
        {
            db.SaveChanges();
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            Category category = db.Categories.FirstOrDefault(u => u.Id == transaction.CategoryId);
            Budget budget = db.Budgets.FirstOrDefault(u => u.Id == category.BudgetId);
            Account account = db.Accounts.FirstOrDefault(u => u.HouseHoldId == user.HouseHoldId);
            if (budget != null)
            {
                budget.TransactionAmount -= transaction.Amount;
            }
            account.Balance += transaction.Amount;
            db.SaveChanges();
        }

        public void AmountChanged (Transaction transaction)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            Budget budget = db.Budgets.FirstOrDefault(u => u.Id == transaction.CategoryId);
            Account account = db.Accounts.FirstOrDefault(u => u.HouseHoldId == user.HouseHoldId);
            var changed = db.Transactions.AsNoTracking().Where(u => u.Id == transaction.Id);
            var originaltrans = changed.FirstOrDefault().Amount;
            if (originaltrans != transaction.Amount)
            {
                if (transaction.TransactionTypeId == 1)
                {
                    account.Balance = account.Balance - originaltrans + transaction.Amount;
                    budget.Amount = budget.Amount - originaltrans + transaction.Amount;
                    db.SaveChanges();
                }
                else
                {
                    account.Balance = account.Balance + originaltrans - transaction.Amount;
                    budget.Amount = budget.Amount + originaltrans - transaction.Amount;
                    db.SaveChanges();
                }
            }
        }

        public double ActualExpense()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var budget = db.Budgets.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            double total = 0;
            foreach (var item in budget)
            {
                total += item.Amount;
            }

            return (total);
        }

        public double BudgetedExpense()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            var budget = db.Budgets.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
            double total = 0;
            foreach (var item in budget)
            {
                total += item.TransactionAmount;
            }

            return (total);
        }

        public double Total(int num)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            return (db.Budgets.Where(x => x.FrequencyId == num && x.HouseHoldId == user.HouseHoldId).Select(u => u.TransactionAmount).DefaultIfEmpty().Sum());
        }
    }
}