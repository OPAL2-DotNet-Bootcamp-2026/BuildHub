using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dtos
{
    /// <summary>
    /// Registration payload. The caller sends a plain password; only its hash is stored.
    /// </summary>
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Homeowner or Vendor. EnumDataType also rejects 0, which is not a defined
        /// role - so a caller who omits this gets a 400 instead of silently becoming
        /// whatever the first enum member happens to be.
        /// </summary>
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be Homeowner or Vendor.")]
        [DeniedValues(UserRole.Admin, ErrorMessage = "Admin accounts cannot be self-registered.")]
        public UserRole Role { get; set; }

        [Required]
        [MaxLength(80)]
        public string City { get; set; } = string.Empty;
    }
}
