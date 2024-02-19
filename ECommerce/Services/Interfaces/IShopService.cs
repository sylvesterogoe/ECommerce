using ECommerce.Models;

namespace ECommerce.Services.Interfaces;

public interface IShopService
{
    Task<CartItem> AddItemToCartAsync(CartItem cartItem, string phoneNumber, int cartId);
    CartItem? GetCartItem(int cartItemId);
    Task<CartItem?> RemoveCartItem(int cartItemId);
    List<CartItem> GetCartItems(string phoneNumber, DateTime time, int quantity, int itemId);
}
