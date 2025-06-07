using backend.Data.IRepository;
using backend.DataAccess.IRepository;
using backend.Models;
using backend.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public OrdersController(IOrderRepository orderRepo, ICartRepository cartRepo, IProductRepository productRepository)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepository;
        }


        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots()
        {
            var cartItems = await _cartRepo.GetAllCartItemsAsync();
            if (cartItems == null || cartItems.Count == 0)
                return BadRequest("Cart is empty.");

            var now = DateTime.Now;
            var greenHours = new List<int> { 13, 14, 20, 21 };
            var products = cartItems.Select(ci => ci.Product).ToList();

            bool hasExternal = products.Any(p => p.Type == ProductType.External);
            bool hasFresh = products.Any(p => p.Type == ProductType.Fresh);
            bool hasInStock = products.Any(p => p.Type == ProductType.InStock);

            DateTime earliest = now;

            // External: Requires 3 days + Tue-Fri
            if (hasExternal)
            {
                earliest = earliest.Date.AddDays(3);
                while (earliest.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday or DayOfWeek.Monday)
                    earliest = earliest.AddDays(1);
            }

            // Fresh: Cutoff is 12:00 same-day
            var minFreshDate = hasFresh && now.Hour >= 12 ? now.Date.AddDays(1) : now.Date;

            // InStock: Cutoff is 18:00 same-day
            var minInStockDate = hasInStock && now.Hour >= 18 ? now.Date.AddDays(1) : now.Date;

            // Take the maximum of all earliest constraints
            earliest = new[] { earliest.Date, minFreshDate, minInStockDate }.Max();

            var slots = new List<DeliverySlotResult>();

            for (int day = 0; day < 14; day++)
            {
                var date = earliest.Date.AddDays(day);

                if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                    continue;

                if (hasExternal && date.DayOfWeek == DayOfWeek.Monday)
                    continue;

                for (int hour = 8; hour < 22; hour++)
                {
                    var slot = date.AddHours(hour);

                    
                    if (slot <= now)
                        continue;

                    bool isGreen = greenHours.Contains(hour);

                    slots.Add(new DeliverySlotResult
                    {
                        Slot = slot,
                        IsGreen = isGreen
                    });
                }
            }

            var sortedSlots = slots
                .OrderBy(s => s.Slot.Date)
                .ThenByDescending(s => s.IsGreen)
                .ThenBy(s => s.Slot.TimeOfDay)
                .ToList();

            return Ok(sortedSlots);
        }


        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] DateTime selectedDeliverySlot)
        {
            var cartItems = await _cartRepo.GetAllCartItemsAsync();
            if (!cartItems.Any())
                return BadRequest("Cart is empty.");

            bool slotTaken = await _orderRepo.IsSlotAlreadyBookedAsync(selectedDeliverySlot);
            if (slotTaken)
                return BadRequest("Selected delivery slot is already booked.");

            
            var orderDetails = cartItems.Select(ci => new OrderDetail
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList();

            
            decimal totalPrice = orderDetails.Sum(od => od.Quantity * od.UnitPrice);

            
            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                DeliveryDateTime = selectedDeliverySlot,
                OrderDetails = orderDetails,
                TotalPrice = totalPrice
            };

            await _orderRepo.CreateOrderAsync(order);
            await _cartRepo.ClearCartAsync();

            return Ok(new
            {
                order.Id,
                order.DeliveryDateTime,
                order.TotalPrice,
                Items = order.OrderDetails.Select(od => new {
                    od.ProductId,
                    od.Quantity,
                    od.UnitPrice
                })
            });
        }

       
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
