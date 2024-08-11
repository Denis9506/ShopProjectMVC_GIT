using System.ComponentModel.DataAnnotations;

namespace ShopProjectMVC.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public int Price { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string? Image { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Count is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Count must be at least 1.")]
        public int Count { get; set; }
    }
}
