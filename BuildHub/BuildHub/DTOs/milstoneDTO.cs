
using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class milstoneDTO
    {

        //Input

        public class CreateMilestoneDto
        {

            [Required(ErrorMessage = "Contract Id is required .")]
            public int contractId {  get; set; }


            [Required(ErrorMessage = "Titel is required .")]
            public string titel {  get; set; }

            [Required]
            public DateTime dueDate { get; set; }


        }
        public class UbdateMilestoneDto
        {
            [Required]
            public DateTime dueDate { get; set; }
        }







        //OutPut
        public class MilestoneOutputDto
        {
            public int milestoneId {  get; set; }
            public int contractId { get; set; }
            public string title { get; set; }
            public decimal amount { get; set; }
            public int orderIndex { get; set; }
            public string status { get; set; }
            public DateTime? dueDate { get; set; }
        }





    }
}
