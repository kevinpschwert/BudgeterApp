using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class InviteHouseHoldViewModel
    {
        public List<HouseHold> HouseHold { get; set; }
        public List<Invitation> Invitation { get; set; }
    }
}