namespace ShopProjectMVC.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }    
        public DateTime CreatedAt { get; set; }
    }
}
