namespace BuildHub.Models;

public class Product
{
    public int productId { get; set; }
    public int vendorProfileId { get; set; }
    public int categoryId { get; set; }
    public string name { get; set; }
    public string unit { get; set; }
    public int currentPrice { get; set; }
    public bool isAvailable { get; set; }

}