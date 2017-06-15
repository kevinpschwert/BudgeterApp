using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class BankAccountViewModel
    {
        //public Account Account { get; set; }
        public List<Transaction> Transaction { get; set; }
    }
}