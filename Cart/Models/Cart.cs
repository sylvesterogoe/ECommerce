using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
