using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class BudgetHistory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Name { get; set; }
        public int HouseHoldId { get; set; }
        public int FrequencyId { get; set; }
        public double TransactionAmount { get; set; }
        public double Budgeted { get; set; }

        public virtual HouseHold HouseHold { get; set; }
        public virtual Frequency Frequency { get; set; }
    }
}