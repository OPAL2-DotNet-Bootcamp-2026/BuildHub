namespace BuildHub.Models;

public class VendorProfile
{
    
    public int VendorProfileID { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; }
    public VendorType VendorType { get; set; }
    public string City { get; set; }
    public bool IsVerfied { get; set; }
    public double AverageRating { get; set; }
    public double Balance { get; set; }
}