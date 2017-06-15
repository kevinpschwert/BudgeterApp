using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Category
    {
        public Category()
        {
            Transaction = new HashSet<Transaction>();            
        }

        public int Id { get; set; }
        public string Name { get; set; }   
        public int? HouseHoldId { get; set; }  
        public int? BudgetId { get; set; }         

        public virtual Budget Budget { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public virtual HouseHold HouseHold { get; set; }

    }
}