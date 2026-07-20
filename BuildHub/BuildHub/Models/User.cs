using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BuildHub.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } //system generated 
        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } //user input 
        [Required]
        [MaxLength(150)]
        public string Email { get; set; } //user input 
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } //system calculated
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }  //user input 
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } //user input
        [Required]
        [MaxLength(100)]
        public string City { get; set; } //user input
        [Required]
        public bool IsVerified { get; set; } = false; //user input
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //default value
    }
}
