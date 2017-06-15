using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class HouseHoldViewModel
    {
        public List<Invitation> Invitation { get; set; }
        public List<HouseHold> HouseHold { get; set; }
        public List<Category> Category { get; set; }
        public List<Transaction> Transaction { get; set; }
        public List<BudgetHistory> BudgetHistory { get; set; }
    }
}