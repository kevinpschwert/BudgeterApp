using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class HouseHold
    {
        public HouseHold()
        {
            Account = new HashSet<Account>();
            Budget = new HashSet<Budget>();
            User = new HashSet<ApplicationUser>();
            Category = new HashSet<Category>();
            Invitation = new HashSet<Invitation>();
            BudgetHistory = new HashSet<BudgetHistory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Budget> Budget { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<Invitation> Invitation { get; set; }
        public virtual ICollection<BudgetHistory> BudgetHistory { get; set; }
    }
}