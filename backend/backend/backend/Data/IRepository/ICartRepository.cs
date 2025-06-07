using backend.Models;

namespace backend.Data.IRepository
{
    public interface ICartRepository
    {

        Task<List<CartItem>> GetAllCartItemsAsync();
        Task AddToCartAsync(CartItem item);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync();
    }

}
