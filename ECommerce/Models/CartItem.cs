namespace ECommerce.Models;

public class CartItem
{
    public int Id { get; set; }
    public Item Item { get; set; }
    public int Quantity { get; set; }
    public DateTime Time { get; set; }
}
