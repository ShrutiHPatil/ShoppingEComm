using System.ComponentModel.DataAnnotations;

namespace ECommSiteApis.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        // Navigation property
        public List<CartItems> CartItems { get; set; }
    }
}
