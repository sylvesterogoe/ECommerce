using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using ECommerce.Services.Interfaces;

namespace ECommerce.Services;

public class ShopService : IShopService
{
    private readonly IUserRepository _userRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ICartItemRepository _cartItemRepository;


    public ShopService(
        IUserRepository userRepository,
        ICartRepository cartRepository,
        ICartItemRepository cartItemRepository)
    {
        _userRepository = userRepository;
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
    }

    public async Task AddUser(User newUser)
    {
        if(newUser.PhoneNumber.Length < 10) throw new ArgumentException("PhoneNumber must be exactly 10 digits"); 
        await _userRepository.AddUserAsync(newUser);
    }

    public async Task AddCart(Cart cart)
    {
        await _cartRepository.AddCartAsync(cart);
    }

    public async Task<CartItem> AddItemToCartAsync(CartItem cartItem, int cartId)
    {
        if (cartItem == null || cartItem.Item == null) throw new ArgumentException("Invalid CartItem or Item");

        var cart = _cartRepository.GetCartById(cartId);
        if (cart == null) throw new ArgumentException("Cart does not exist");

        var exstingCartItem = cart.CartItems?.Where(ci => ci.Id == cartItem.Id).FirstOrDefault();
        if (exstingCartItem != null)
        {
            var updatedCartItem = await _cartItemRepository.UpdateCartItemQuantity(exstingCartItem, cartItem.Quantity);
            return updatedCartItem;
        }

        var newCartItem = new CartItem { Id = cartItem.Id, Item = cartItem.Item, Quantity = cartItem.Quantity, Time = cartItem.Time};
        await _cartItemRepository.AddCartItem(cart, newCartItem);
        return newCartItem;
    }

    public List<CartItem> GetAllCartItems(int cartId)
    {
        var cart = _cartRepository.GetCartById(cartId);

        var cartItems = cart?.CartItems;
        if (cartItems == null || !cartItems.Any()) return new List<CartItem>();

        return cartItems;
    }

    public List<CartItem> GetCartItemsWithFiltering(
        string phoneNumber, DateTime time, int quantity, int itemId)
    {
        var emptyList = new List<CartItem>();

        //Find user with the PhoneNumber
        var user = _userRepository.GetUserByPhoneNumber(phoneNumber);
        if (user == null) return emptyList;

        //Find cart belonging to the user
        var cart = _cartRepository.GetCartByUserId(user.Id);
        if (cart == null) return emptyList;

        //Find cartItems on the cart with given time and quantiy
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