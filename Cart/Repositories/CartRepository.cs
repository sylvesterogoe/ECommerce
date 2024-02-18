using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public Cart? GetCartById(int cartId)
        {
            var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Item).FirstOrDefault(c => c.Id == cartId);
            return cart;
        }
    }
}
