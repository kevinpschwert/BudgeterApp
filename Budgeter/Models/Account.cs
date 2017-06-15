using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Account
    {
        public Account()
        {
            Transaction = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public double ReconciledBalance { get; set; }

        public virtual HouseHold HouseHold { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}