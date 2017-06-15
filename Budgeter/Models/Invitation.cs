using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public bool Accepted { get; set; }
        public bool Expired { get; set; }
        public DateTimeOffset Generated { get; set; }
        public DateTimeOffset? Expiration { get; set; }
        public int HouseHoldId { get; set; }

        public virtual HouseHold HouseHold { get; set; }
    }
}