using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class TransactionViewModel
    {
        public List<Transaction> Transaction { get; set; }
        public List<BudgetHistory> BudgetHistory { get; set; }
    }
}