using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly ApplicationDbContext _context;

    public CartItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public CartItem? GetCartItem(int cartItemId)
    {
        var cartItem = _context.CartItems.Include(ci => ci.Item).Where(ci => ci.Id == cartItemId).FirstOrDefault();
        return cartItem;
    }

    public async Task AddCartItem(Cart cart, CartItem newCartItem)
    {
        await _context.AddAsync(newCartItem);
        cart.CartItems?.Add(newCartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<CartItem> UpdateCartItemQuantity(CartItem exstingCartItem, int Quantity)
    {
        _context.Update(exstingCartItem);
        exstingCartItem.Quantity += Quantity;
        await _context.SaveChangesAsync();
        return exstingCartItem;
    }

    public async Task DeleteCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }
}
