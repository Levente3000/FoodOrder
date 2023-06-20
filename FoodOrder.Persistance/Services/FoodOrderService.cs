using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Persistance.Services
{
    public class FoodOrderService : IFoodOrderService
    {
        private readonly FoodOrderDbContext _context;

        public FoodOrderService(FoodOrderDbContext context)
        {
            _context = context;
        }

        public IList<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public IList<Products> GetTop10Products()
        {
            IList<Products> r = new List<Products>();

            var tmp = _context.ProductOrders.GroupBy(t => t.ProductID, (key, list) => new { ProductId = key, count = list.Count() }).OrderByDescending(t => t.count).Take(10).ToList();

            foreach (var item in tmp)
            {
                r.Add(_context.Products.Find(item.ProductId)!);
            }

            return r;
        }

        public IList<Products> GetAllProducts(string categoryName)
        {
            IList<Products> products = new List<Products>();
            foreach (var p in _context.Products)
            {
                if (p.CategoryName == categoryName)
                {
                    products.Add(p);
                }
            }

            return products;
        }

        public int GetCurrentOrderId()
        {
            return _context.Orders.Count();
        }

        public bool SaveOrder(Orders order)
        {
            int tmpid = order.OrderId;

            _context.Orders.Add
            (
                new Orders
                {
                    OrdererName = order.OrdererName,
                    Address = order.Address,
                    PhoneNumber = order.PhoneNumber,
                    Done = order.Done,
                    RegistrationDate = order.RegistrationDate,
                    DoneDate = null,
                    SumPrice = order.SumPrice,
                }
            );

            foreach (var p in order.Products)
            {
                foreach (var item in _context.Products)
                {
                    if (p.Id == item.Id)
                    {
                        _context.ProductOrders.Add
                        (
                            new ProductOrder
                            {
                                OrderID = tmpid,
                                ProductID = item.Id
                            }
                        );
                    }
                }
            }


            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool UpdateOrder(Orders order)
        {
            try
            {
                _context.Update(order);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public IList<Orders> GetOrders()
        {
            return _context.Orders.ToList();
        }

        public Orders GetOrder(int id)
        {
            return _context.Orders.FirstOrDefault(p => p.OrderId == id)!;
        }

        public async Task<Products>? CreateProduct(Products? product)
        {
            if (product == null)
            {
                return null!;
            }

            if (_context.Products.Any(p => p.Name == product.Name) || product.Price <= 0)
            {
                return null!;
            }

            foreach (var item in _context.Categories)
            {
                if(item.Name == product.CategoryName)
                {
                    product.Category = item;
                }
            }
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null!;
            }

            return product;
        }

        public bool UpdateProducts(Products product)
        {
            try
            {
                _context.Update(product);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public Products GetProduct(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id)!;
        }

        public IList<Products> GetAllProductsForOrder(int orderId)
        {
            IList<Products> products = new List<Products>();
            foreach (var p in _context.ProductOrders)
            {
                if (p.OrderID == orderId)
                {
                    products.Add(p.Product);
                }
            }

            return products;
        }

        public IList<Products> GetAllProducts()
        {
            IList<Products> products = new List<Products>();
            foreach (var p in _context.Products)
            {
                products.Add(p);
            }

            return products;
        }

        public bool CategoryIsDrink(string name)
        {
            var result = _context.Categories.FirstOrDefault(c => c.Name == name);

            if(result == null)
            {
                return false;
            }

            return result.IsDrink;
        }

        public IList<Orders> LoadDoneOrdersAsync()
        {
            return _context.Orders.Where(o => o.Done == true).ToList();
        }

        public IList<Orders> LoadUnDoneOrdersAsync()
        {
            return _context.Orders.Where(o => o.Done == false).ToList();
        }

        public IList<Orders> LoadOrdersByNameAsync(string name)
        {
            var l = _context.Orders.Where(p => p.OrdererName.ToLower().Contains(name.ToLower())).ToList();
            return l;
        }

        public IList<Orders> LoadOrdersByAddressAsync(string address)
        {
            return _context.Orders.Where(p => p.Address.ToLower().Contains(address.ToLower())).ToList();
        }
    }
}
