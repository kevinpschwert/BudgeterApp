using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class AccountsViewModel
    {
        public List<Account> Account { get; set; }
        public List<BudgetHistory> BudgetHistory { get; set; }
    }
}