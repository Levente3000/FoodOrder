using AutoMapper;
using FoodOrder.DTO;
using FoodOrder.Persistance;
using FoodOrder.Persistance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FoodOrder.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IFoodOrderService _service;
        private readonly IMapper _mapper;

        public OrdersController(IFoodOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            return _service
                .GetOrders()
                .Select(order => _mapper.Map<OrderDto>(order))
                .ToList();
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<ProductDto>> GetProducts(int id)
        {
            try
            {
                return _service
                    .GetAllProductsForOrder(id)
                    .Select(product => _mapper.Map<ProductDto>(product))
                    .ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("getdoneorders")]
        public ActionResult<IEnumerable<OrderDto>> GetDoneOrders()
        {
            try
            {
                return _service
                   .LoadDoneOrdersAsync()
                   .Select(order => _mapper.Map<OrderDto>(order))
                   .ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("getundoneorders")]
        public ActionResult<IEnumerable<OrderDto>> GetUnDoneOrders()
        {
            try
            {
                return _service
                    .LoadUnDoneOrdersAsync()
                    .Select(order => _mapper.Map<OrderDto>(order))
                    .ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("getordersbyname/{searchBar}")]
        public ActionResult<IEnumerable<OrderDto>> GetOrdersByName(string searchBar)
        {
            try
            {
                return _service
                    .LoadOrdersByNameAsync(searchBar)
                    .Select(order => _mapper.Map<OrderDto>(order))
                    .ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("getordersbyaddress/{searchBar}")]
        public ActionResult<IEnumerable<OrderDto>> GetOrdersByAddress(string searchBar)
        {
            try
            {
                return _service
                    .LoadOrdersByAddressAsync(searchBar)
                    .Select(order => _mapper.Map<OrderDto>(order))
                    .ToList();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult PutOrder(Int32 id, OrderDto orderDto)
        {
            if (id != orderDto.OrderId)
            {
                return BadRequest();
            }

            if (_service.UpdateOrder(_mapper.Map<Orders>(orderDto)))
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
