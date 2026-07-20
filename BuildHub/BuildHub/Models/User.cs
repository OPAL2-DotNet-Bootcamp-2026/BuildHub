namespace BuildHub.Models
{
    public class User
    {
        public int Id { get; set; } //system generated 
        public int UserId { get; set; } //system generated 
        public string FullName { get; set; } //user input 
        public string Email { get; set; } //user input 
        public string PasswordHash { get; set; } //system calculated
        public string? PhoneNumber { get; set; }  //user input 
        public string PhoneNumber { get; set; }  //user input 
        public string Role { get; set; } //user input
        public string City { get; set; } //user input
        public bool IsVerified { get; set; } = false; //user input
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //user input
    }
}
