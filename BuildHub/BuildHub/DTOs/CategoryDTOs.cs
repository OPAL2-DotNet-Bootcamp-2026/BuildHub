using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{

    // Input DTOs 
    public class CategoryInputDTOs
    {
        [Required(ErrorMessage = "This field is required")]
        public string nameAr { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string nameEn { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string type { get; set; }
    }



    // Output DTOs 
    public class CategoryOutputDTOs
    {
        public string nameAr { get; set; }
        public string nameEn { get; set; }
        public string type { get; set; } 
    }

}
