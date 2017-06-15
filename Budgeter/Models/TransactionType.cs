using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class TransactionType
    {
        public TransactionType()
        {
            Transaction = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Transaction> Transaction { get; set; }
    }
}