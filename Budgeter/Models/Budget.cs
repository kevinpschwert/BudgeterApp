using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Budget
    {
        public Budget()
        {
            Category = new HashSet<Category>();
        }

        public int Id { get; set; }
        public int HouseHoldId { get; set; }
        public string Name { get; set; }
        public int FrequencyId { get; set; }
        public double Amount { get; set; }
        public double ConstAmount { get; set; }
        public double TransactionAmount { get; set; }
        public DateTimeOffset Expire { get; set; }

        public virtual HouseHold HouseHold { get; set; }
        public ICollection<Category> Category { get; set; }
        public virtual Frequency Frequency { get; set; }
    }
}