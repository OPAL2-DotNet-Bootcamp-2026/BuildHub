using BuildHub.enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildHub.DTOs
{
    public class VendorProfileResponseDTO
    {
        public int VendorProfileID { get; set; } 
        public int UserId { get; set; }
        public string CompanyName { get; set; } 
        public VendorType VendorType { get; set; }
        public string City { get; set; } 
        public bool IsVerfied { get; set; } 
        public decimal? AverageRating { get; set; }  
        public decimal Balance { get; set; }
    }
}
