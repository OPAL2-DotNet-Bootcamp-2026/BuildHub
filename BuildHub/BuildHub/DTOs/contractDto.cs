using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class contractDto
    {
        //Input
        public class ContractInputDto
        {

            [Required]
            public int quoteId {  get; set; }



            [Required]
            [Range(0, double.MaxValue, ErrorMessage = "Should be more than 0")]
            public decimal totalAmount { get; set; }



            [Required(ErrorMessage = "Payment Type is required .")]
            [AllowedValues("One time ", "PreMilestone", ErrorMessage = "Invalid payment type")]
            public string paymentType { get; set; }

        }


        // Output

        public class ContractOutputDTO
        {
            public int contractId { get; set; }
            public int quoteId { get; set; }

            public decimal totalAmount { get; set; }

            public string paymentType { get; set; }

            public string status { get; set; }

            public DateTime signedAt { get; set; }


        }

        public class DetailsOutputDto
        {
            public int contractId { get; set; }
            public decimal totalAmount { get; set; }

            public string paymentType { get; set; }
            public string status { get; set; }

            public DateTime signedAt { get; set; }

        }
        

        public class UpdateStatusDto
        {
            public string status { get; set; }
        }
  













    }
}
