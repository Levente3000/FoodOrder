using AutoMapper;
using FoodOrder.DTO;
using FoodOrder.Persistance;
using FoodOrder.Persistance.Services;
using FoodOrder.WebAPI.Controllers;
using FoodOrder.WebAPI.MappingConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.WebAPI.Tests
{
    public class ControllerTest : IDisposable
    {
        private readonly FoodOrderDbContext _context;
        private readonly FoodOrderService _service;
        private readonly OrdersController? _orderController;
        private readonly ProductsController? _productController;
        private readonly AccountController? _accountController;
        private Mock<SignInManager<Employees>>? _signInManager;
        private readonly IMapper _mapper;

        public ControllerTest()
        {
            var options = new DbContextOptionsBuilder<FoodOrderDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _context = new FoodOrderDbContext(options);
            TestDbInitializer.Initialize(_context, ref _signInManager);

            _context.ChangeTracker.Clear();
            _service = new FoodOrderService(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrderProfile());
                cfg.AddProfile(new OrderDtoProfile());
                cfg.AddProfile(new ProductsProfile());
                cfg.AddProfile(new ProductDtoProfile());
            });
            _mapper = new Mapper(config);
            _orderController = new OrdersController(_service, _mapper);
            _productController = new ProductsController(_service, _mapper);
            _accountController = new AccountController(_signInManager!.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region OrderController

        [Fact]
        public void GetOrdersTest()
        {
            var result = _orderController!.GetOrders();

            var content = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(result.Value);
            Assert.Equal(2, content.Count());
        }

        [Fact]
        public void PutDoneOrderBadRequestTest()
        {
            var id = 4;

            var order1 = new OrderDto { OrderId = 1, Done = true, };

            var result = _orderController!.PutOrder(id, order1);

            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void PutDoneOrderTest(int id)
        {
            var previousOrder = _orderController!.GetOrders().Value!.FirstOrDefault(p => p.OrderId == id);

            Assert.False(previousOrder?.Done);
            previousOrder!.Done = true;
            previousOrder.DoneDate = DateTime.Now;

            _context.ChangeTracker.Clear();

            var result = _orderController!.PutOrder(id, _mapper.Map<OrderDto>(previousOrder));

            var requestResult = Assert.IsAssignableFrom<OkResult>(result);
            var updatedOrder = _orderController.GetOrders().Value!.FirstOrDefault(p => p.OrderId == id);
            Assert.True(updatedOrder?.Done);
        }

        #endregion

        #region ProductController

        [Fact]
        public void GetProductsTest()
        {
            var result = _productController!.GetProducts();

            var content = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result.Value);
            Assert.Equal(12, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetProductsForOrderTest(int id)
        {
            var result = _orderController!.GetProducts(id);

            var content = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result.Value);
            if (id == 1)
            {
                Assert.Equal(3, content.Count());
            }
            else if (id == 2)
            {
                Assert.Equal(4, content.Count());
            }
            else if (id == 3)
            {
                Assert.Empty(content);
            }
        }

        [Fact]
        public void GetProduct1Test()
        {
            var result1 = _productController!.GetProduct(1);

            Assert.Equal("Sajtkrémleves", result1.Value!.Name);
        }

        [Fact]
        public void GetProduct4Test()
        {
            var result2 = _productController!.GetProduct(4);

            Assert.Equal("Pepperoni", result2.Value!.Name);
        }

        [Fact]
        public void GetProduct0Test()
        {
            var result3 = _productController!.GetProduct(0);

            Assert.Null(result3.Value);
        }

        [Fact]
        public void PostProductTest()
        {
            var newProduct = new ProductDto
            {
                Name = "Sonkás",
                CategoryName = "Pizzák",
                Description = "Description",
                Price = 1200,
                Spicy = false,
                Vegetarian = false,
            };

            var count = _context.Products.Count();
            Assert.Equal(12, count);

            var result = _productController!.PostProduct(newProduct);

            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<ProductDto>(objectResult.Value);

            Assert.Equal(count + 1, _context.Products.Count());
        }

        [Fact]
        public void PostProductWithSameNameTest()
        {
            var newProduct = new ProductDto
            {
                Name = "Magyaros",
                CategoryName = "Pizzák",
                Description = "Description",
                Price = 1200,
                Spicy = false,
                Vegetarian = false,
            };

            var count = _context.Products.Count();
            Assert.Equal(12, count);

            var result = _productController!.PostProduct(newProduct);

            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            Assert.Null(objectResult.Value);
        }

        #endregion

        #region AccountController

        [Fact]
        public async Task LoginTest()
        {
            var loginDto = new LoginDto()
            {
                UserName = "admin1",
                Password = "Testpw123"
            };

            await _accountController!.Login(loginDto);

            _signInManager!.Verify(mock => mock.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false), Times.Once);
        }

        [Fact]
        public async Task LogoutTest()
        {
            await _accountController!.Logout();

            _signInManager!.Verify(mock => mock.SignOutAsync());
        }

        #endregion
    }
}
