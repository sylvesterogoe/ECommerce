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

        var newCartItem = new CartItem { Id = cartItem.Id, Item = cartItem.Item, Quantity = cartItem.Quantity };
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

    //public async List<CartItem> FilterCartItemsAsync<T>(T filter)
    //{
    //    CartItem result = filter switch
    //    {
    //        string => _cartItemRepository.GetCartItem(4)
    //    };
    //}

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