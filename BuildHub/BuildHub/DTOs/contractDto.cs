using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class contractDto
    {
        //Input
        public class ContractInputDto
        {

            [Required(ErrorMessage = "Contract Id is required .")]
            public int quoteId {  get; set; }



            [Required(ErrorMessage = "Contract Id is required .")]
            public decimal totalAmount { get; set; }



            [Required(ErrorMessage = "Contract Id is required .")]
            public string paymentType { get; set; }

        }













    }
}
