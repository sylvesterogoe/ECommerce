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
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public Cart? GetCartById(int cartId)
        {
            var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Item).FirstOrDefault(c => c.Id == cartId);
            return cart;
        }

        public Cart? GetCartByUserId(int userId)
        {
            return _context.Carts.Include(c => c.CartItems).ThenInclude(c => c.Item).Where(c => c.UserId == userId).FirstOrDefault();
        }
    }
}
