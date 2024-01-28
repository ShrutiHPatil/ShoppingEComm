using System.ComponentModel.DataAnnotations;

namespace ECommSiteApis.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        // Navigation property
        public List<CartItems> CartItems { get; set; }
    }
}
