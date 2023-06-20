using AutoMapper;
using FoodOrder.DTO;
using FoodOrder.Persistance;
using FoodOrder.Persistance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IFoodOrderService _service;
        private readonly IMapper _mapper;

        public ProductsController(IFoodOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                return _service
                    .GetAllProducts()
                    .Select(product => _mapper.Map<ProductDto>(product))
                    .ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("product/{id}")]
        public ActionResult<ProductDto> GetProduct(int id)
        {
            try
            {
                var p = _service.GetProduct(id); 
                return _mapper.Map<ProductDto>(p);

            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("Isdrink/{name}")]
        public ActionResult<bool> GetCategoryIsDrink(string name)
        {
            try
            {
                var r = _service.CategoryIsDrink(name);
                return r;
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult<ProductDto> PostProduct(ProductDto productDto)
        {
            var newItem = _service.CreateProduct(_mapper.Map<Products>(productDto));

            if (newItem is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return CreatedAtAction(nameof(GetProduct), new { id = newItem.Id },
                    _mapper.Map<ProductDto>(newItem.Result));
            }
        }

    }
}

