using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Entities
{
    public class Payment
    {
            public int PaymentId { get; set; }

            
            public int AgreementId { get; set; }

       
            public int HomeownerId { get; set; }

          
            public decimal AgreementAmount { get; set; }

        
            public decimal BuildHubFee { get; set; }

       
            public decimal TotalAmount { get; set; }

          
            public string Currency { get; set; } = "OMR";

            public GatewayPaymentStatus Status { get; set; }

           
            public string ClientReferenceId { get; set; } = string.Empty;

            
            public string? GatewaySessionId { get; set; }


            public string? CheckoutUrl { get; set; }

      
            public string? GatewayPaymentId { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime? PaidAt { get; set; }

            public Agreement Agreement { get; set; } = null!;

            public User Homeowner { get; set; } = null!;
        
        }
    }

