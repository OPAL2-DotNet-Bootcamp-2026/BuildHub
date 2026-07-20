namespace BuildHub.Models;

public class Category
{
    
    public int categoryId { get; set; }
    public int parentCategoryId { get; set; }
    public string nameAr {  get; set; }
    public string nameEn { get; set; }
    public string type {  get; set; }
}