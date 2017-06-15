using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class DashboardViewModel
    {
        public List<Account> Account { get; set; }
        public List<Transaction> Transaction { get; set; }
        public List<Budget> Budget { get; set; }
        public List<Notification> Notification { get; set; }
        public List<BudgetHistory> BudgetHistory { get; set; }
    }
}