namespace ShopProjectMVC.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public virtual Category Category { get; set; }
        public int Count { get; set; }
    }
}

