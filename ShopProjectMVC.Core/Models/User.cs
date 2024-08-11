using System.ComponentModel.DataAnnotations;

namespace ShopProjectMVC.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long.")]
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Role Role { get; set; }  
    }
}

public enum Role { 
    Client,
    Admin
}