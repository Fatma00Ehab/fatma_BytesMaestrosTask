using backend.Data.IRepository;
using backend.DataAccess;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync()
        {
            return await _context.CartItems.Include(c => c.Product).ToListAsync();
        }

        public async Task AddToCartAsync(CartItem item)
        {
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                await _context.CartItems.AddAsync(item);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync()
        {
            var allItems = _context.CartItems.ToList();
            _context.CartItems.RemoveRange(allItems);
            await _context.SaveChangesAsync();
        }
    }





}
