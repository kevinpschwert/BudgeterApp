using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Transaction
    {
       
        public int Id { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        [Required]
        public DateTimeOffset Date { get; set; }
        public double Amount { get; set; }
        public string EnteredById { get; set; }
        public int? CategoryId { get; set; }
        public bool Reconciled { get; set; }
        public double ReconciledAmount { get; set; }
        public int TransactionTypeId { get; set; }
        public bool Void { get; set; }

        public virtual TransactionType TransactionType { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser EnteredBy { get; set; }
        public virtual Account Account { get; set; } 
        
            
    }
}