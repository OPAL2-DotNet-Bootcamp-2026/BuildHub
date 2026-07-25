using System.ComponentModel.DataAnnotations;

namespace BuildHub.DTOs
{
    public class UserUpdateDTO 
    {
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(150)]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }  
    }

    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }  
    }
}
