using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class BudgetViewModel
    {
        public List<Budget> Budget { get; set; }
        public List<BudgetHistory> BudgetHistory { get; set; }
    }
}