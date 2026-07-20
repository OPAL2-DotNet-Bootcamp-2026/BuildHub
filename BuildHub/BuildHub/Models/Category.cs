using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int categoryId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? parentCategoryId { get; set; }

    [Required]
    [MaxLength(150)]
    public string nameAr {  get; set; }

    [Required]
    [MaxLength(150)]
    public string nameEn { get; set; }

    [Required]
    [MaxLength(20)]
    public string type {  get; set; }
}