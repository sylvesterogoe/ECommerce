using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services;

public class ShopService : IShopService
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;


    public ShopService(
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task<CartItem> AddItemToCartAsync(CartItem cartItem, string phoneNumber, int cartId)
    {
        if (cartItem == null || cartItem.Item == null) throw new ArgumentException("Invalid CartItem or Item");

        var cart = _cartRepository.GetCartById(cartId);
        if (cart == null)
        {
            await _cartRepository.AddCartAsync(new Cart { Id = cartId, CartItems = new List<CartItem> { cartItem }, PhoneNumber = phoneNumber });
            var createdCart = _cartRepository.GetCartById(cartId);
            var cartItems = createdCart!.CartItems.Where(ci => ci.Id == cartItem.Id).FirstOrDefault();
            return cartItems!;
        }

        var existingCart = _cartRepository.GetCartById(cartId);

        var exstingCartItem = existingCart?.CartItems?.Where(ci => ci.Id == cartItem.Id).FirstOrDefault();
        if (exstingCartItem != null)
        {
            var updatedCartItem = await _cartItemRepository.UpdateCartItemQuantity(exstingCartItem, cartItem.Quantity);
            return updatedCartItem;
        }

        var newCartItem = new CartItem { Id = cartItem.Id, Item = cartItem.Item, Quantity = cartItem.Quantity, Time = cartItem.Time };
        await _cartItemRepository.AddCartItem(cart, newCartItem);
        return newCartItem;
    }

    public List<CartItem> GetCartItems(
        string phoneNumber, DateTime time, int quantity, int itemId)
    {
        var emptyList = new List<CartItem>();

        var cart = _cartRepository.GetCartByPhoneNumber(phoneNumber);
        if (cart == null) return emptyList;

        var cartItems = cart.CartItems.Where(ci => ci.Time == time).Where(ci => ci.Quantity == quantity).Where(ci => ci.Item.Id == itemId).ToList();
        if (cartItems.Count == 0) return emptyList;

        return cartItems;
    }

    public CartItem? GetCartItem(int cartItemId)
    {
        var cartItem = _cartItemRepository.GetCartItem(cartItemId);
        return cartItem;
    }

    public async Task<CartItem?> RemoveCartItem(int cartItemId)
    {
        var cartItem = _cartItemRepository.GetCartItem(cartItemId);
        if (cartItem == null) return null;

        await _cartItemRepository.DeleteCartItem(cartItem);

        return cartItem;
    }
}