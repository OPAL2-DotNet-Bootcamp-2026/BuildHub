using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Profile edit. Only the fields a user may change about themselves appear here;
    /// email, password and role are absent by design, so a caller cannot even ask.
    /// </summary>
    public class UpdateUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = string.Empty;
    }
}
