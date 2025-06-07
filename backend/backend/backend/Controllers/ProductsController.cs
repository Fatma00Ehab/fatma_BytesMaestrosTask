using Microsoft.AspNetCore.Mvc;
using backend.DataAccess.IRepository;
using backend.Models;
using backend.Helpers; 
using Microsoft.AspNetCore.Authorization;
using backend.Models.Dtos;


namespace backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IDataRepository<Product> _repo;

        public ProductsController(IDataRepository<Product> repo)
        {
            _repo = repo;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Type = dto.Type,

                 
            };

            if (dto.Image != null)
            {
                var result = UploadHandler.Upload(dto.Image, "products");
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                    return BadRequest(result.ErrorMessage);

                product.ImageUrl = result.FileName;
            }

            await _repo.AddAsync(product);
            await _repo.Save();

            return Ok(new { message = "Product created." });
        }





        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] string? type)
        {
            var baseUrl = "https://localhost:7148/";
            var products = await _repo.GetAllAsyncInclude(p => true);

            
            if (!string.IsNullOrEmpty(type))
            {
                bool isValid = Enum.TryParse<ProductType>(type, true, out var parsedType);
                if (!isValid)
                    return BadRequest("Invalid product type. Valid values: InStock, Fresh, External.");

                products = products.Where(p => p.Type == parsedType).ToList();
            }

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl) && !product.ImageUrl.StartsWith("http"))
                {
                    product.ImageUrl = baseUrl + product.ImageUrl;
                }
            }

            return Ok(products);
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repo.GetByIdAsyncInclude(p => p.Id == id);

            if (product == null)
                return NotFound();


            if (!string.IsNullOrEmpty(product.ImageUrl) && !product.ImageUrl.StartsWith("http"))
            {
                var baseUrl = "https://localhost:7130/";
                product.ImageUrl = baseUrl + product.ImageUrl;
            }

            return Ok(product);
        }


        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();

            await _repo.DeleteAsync(product);
            await _repo.Save();

            return Ok();
        }
    }

}
