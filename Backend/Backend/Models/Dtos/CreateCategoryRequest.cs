using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    public class CreateCategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;

        [Url]
        [MaxLength(500)]
        public string? IconUrl { get; set; }
    }
}
