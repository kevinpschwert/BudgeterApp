using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public int HouseHoldId { get; set; }
        public string Description { get; set; }

        public virtual HouseHold HouseHold { get; set; }
    }
}