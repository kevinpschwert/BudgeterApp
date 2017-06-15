using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Helper
{
    public class BudgetHistoryHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public double Total(int num, string Title)
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            return (db.BudgetHistories.Where(x => x.FrequencyId == num && x.HouseHoldId == user.HouseHoldId && x.Title == Title && x.Name != "Gifts").Select(u => u.TransactionAmount).DefaultIfEmpty().Sum());
        }

        public void History()
        {
            var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            if (DateTime.Now.ToString("MMMM") != user.LastLogIn.ToString("MMMM"))
            {
                var mybudget = db.Budgets.Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
                var oldbudget = db.Budgets.AsNoTracking().Where(u => u.HouseHoldId == user.HouseHoldId).ToList();
                foreach (var b in oldbudget)
                {
                    BudgetHistory budget = new BudgetHistory();
                    budget.Budgeted = b.ConstAmount;
                    budget.Date = DateTimeOffset.Now;
                    budget.FrequencyId = b.FrequencyId;
                    budget.HouseHoldId = user.HouseHoldId ?? 1;
                    budget.Name = b.Name;
                    budget.TransactionAmount = b.TransactionAmount;
                    if (DateTimeOffset.Now.ToString("MMMM") != "January")
                    {
                        budget.Title = (DateTimeOffset.Now.AddMonths(-1).ToString("MMMM") + " " + DateTimeOffset.Now.ToString("yyyy"));
                    }
                    else
                    {
                        budget.Title = (DateTimeOffset.Now.AddMonths(-1).ToString("MMMM") + " " + DateTimeOffset.Now.AddYears(-1).ToString("yyyy"));
                    }
                    db.BudgetHistories.Add(budget);
                    db.SaveChanges();
                }
                foreach (var m in mybudget)
                {
                    m.TransactionAmount = 0;
                    db.SaveChanges();
                }
            }
        }
    }
}