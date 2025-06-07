using backend.Models;

namespace backend.Data.IRepository
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<List<Order>> GetAllOrdersAsync();
        Task<bool> IsSlotAlreadyBookedAsync(DateTime slot);

    }
}
