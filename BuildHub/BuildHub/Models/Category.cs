using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.Models;

public class Category
{
    // the data annotation
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(ParentCategory))]
    public int? ParentCategoryId { get; set; }

    [Required]
    [MaxLength(150)]
    public string NameAr { get; set; }

    [Required]
    [MaxLength(150)]
    public string NameEn { get; set; }

    [Required]
    [MaxLength(20)]
    public string Type { get; set; }

    // The navigation properties
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
}
