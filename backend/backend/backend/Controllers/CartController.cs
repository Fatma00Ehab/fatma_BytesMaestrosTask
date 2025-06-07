using backend.Data.IRepository;
using backend.DataAccess;
using backend.Models;
using backend.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        private readonly DataContext _context;

        public CartController(ICartRepository cartRepo, DataContext context)
        {
            _cartRepo = cartRepo;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartRepo.GetAllCartItemsAsync();

            string baseUrl = "https://localhost:7148/";

            foreach (var item in cart)
            {
                if (!string.IsNullOrEmpty(item.Product?.ImageUrl) && !item.Product.ImageUrl.StartsWith("http"))
                {
                    item.Product.ImageUrl = baseUrl + item.Product.ImageUrl;
                }
            }

            return Ok(cart);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
             
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return BadRequest($"Product with ID {dto.ProductId} does not exist.");
 
            var cartItem = new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _cartRepo.AddToCartAsync(cartItem);
            return Ok(" Item added to cart.");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _cartRepo.RemoveFromCartAsync(id);
            return Ok("Removed.");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            await _cartRepo.ClearCartAsync();
            return Ok("Cleared.");
        }
    }

}
