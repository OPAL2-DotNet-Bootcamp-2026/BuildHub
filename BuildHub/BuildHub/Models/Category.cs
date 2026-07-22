using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Category
{
    // the data annotation 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int categoryId { get; set; }


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

    // The navigation properties 
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
}