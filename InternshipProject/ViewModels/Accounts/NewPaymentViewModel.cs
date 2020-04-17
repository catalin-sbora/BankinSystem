using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProject.ViewModels.Accounts
{
    public enum NewPaymentStatus { 
        NotInitiated = 0,
        Created,
        Failed
    }
    public class NewPaymentViewModel
    {
        [Required]
        public Guid? SourceAccount { get; set; }
        
        [Required(ErrorMessage = "The amount must be specified")]
        [Display(Name = "Amount")]
        public decimal? Amount { get; set; }

        [Required]
        [Display(Name = "Destination Name")]
        public string  DestinationName { get; set; }

        [Required]
        [Display(Name = "Destination IBAN")]
        public string DestinationIBAN { get; set; }

        public string Details { get; set; }

        public NewPaymentStatus PaymentStatus { get; set; }

        public string PaymentMessage { get; set; }
    }
}
