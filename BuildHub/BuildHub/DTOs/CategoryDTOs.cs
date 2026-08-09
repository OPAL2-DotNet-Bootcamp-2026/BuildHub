using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{

    // This is for input DTOs
    public class CategoryInputDto
    {
        [Required(ErrorMessage = "This field is required")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Type { get; set; }
    }



    // This is for the output DTOs
    public class CategoryOutputDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Type { get; set; }
    }

}
