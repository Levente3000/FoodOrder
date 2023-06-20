using FoodOrder.Persistance;
using FoodOrder.Persistance.Services;
using FoodOrderWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace FoodOrderWeb.Controllers
{
    public class ShoppingCartController : Controller
    {
        protected readonly IFoodOrderService _foodOrderService;

        public ShoppingCartController(IFoodOrderService foodservice)
        {
            _foodOrderService = foodservice;
        }

        public IActionResult Index()
        {
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.Products = new List<Products>();

            orderViewModel.Products = HttpContext.Session.Get<List<Products>>("Cart") ?? new List<Products>();
            int price = 0;

            foreach (var product in orderViewModel.Products)
            {
                price += product.Price;
            }
            ViewBag.sumPrice = price;


            return View("Index", orderViewModel);
        }

        public IActionResult Result(OrderViewModel orderViewModel)
        {
            Orders order = new Orders
            {
                OrderId = (_foodOrderService.GetCurrentOrderId() + 1),
                OrdererName = orderViewModel.Name,
                Products = HttpContext.Session.Get<List<Products>>("Cart") ?? new List<Products>(),
                Address = orderViewModel.Address,
                PhoneNumber = orderViewModel.PhoneNumber,
                Done = orderViewModel.Done,
                RegistrationDate = DateTime.Now,
            };

            int price = 0;
            foreach (var product in order.Products)
            {
                price += product.Price;
            }
            ViewBag.sumPrice = price;

            order.SumPrice = price;

            if (!_foodOrderService.SaveOrder(order))
            {
                ModelState.AddModelError("", "A rendelés rögzítése sikertelen, kérem próbálja újra!");
                return View("Index", orderViewModel);
            }
            
            HttpContext.Session.Clear();

            return View("Details");
        }

        public IActionResult AddFromProducts(int pId, string cn)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("Cart") ?? new List<Products>();
            int price = 0;

            foreach (var product in products)
            {
                price += product.Price;
            }

            IList<Products> plist = _foodOrderService.GetAllProducts(cn).ToList();
            Products? pr = new Products();

            pr = plist.FirstOrDefault(p => p.Id == pId);

            if((price + pr!.Price) <= 20000)
            {
                products.Add(pr);

                HttpContext.Session.Set("Cart", products);
            }
            else
            {
                TempData["ErrorMessage"] = "Ezt az ételt/italt nem teheti a kosárba mert eléri a 20000 Ft-os limitet.";
                return RedirectToAction("ProductList", "Home", new { cn = cn });
            }

            return RedirectToAction("ProductList", "Home", new {cn = cn});
        }

        public IActionResult AddFromMain(int pId, string cn)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("Cart") ?? new List<Products>();
            int price = 0;

            foreach (var product in products)
            {
                price += product.Price;
            }

            IList<Products> plist = _foodOrderService.GetAllProducts(cn).ToList();
            Products? pr = new Products();

            pr = plist.FirstOrDefault(p => p.Id == pId);

            if ((price + pr!.Price) <= 20000)
            {
                products.Add(pr);

                HttpContext.Session.Set("Cart", products);
            }
            else
            {
                TempData["ErrorMessage"] = "Ezt az ételt/italt nem teheti a kosárba mert eléri a 20000 Ft-os limitet.";
                return RedirectToAction("Index", "Home", new { cn = cn });
            }

            return RedirectToAction("Index", "Home", new { cn = cn });
        }

        public IActionResult Delete(int pId)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("Cart") ?? new List<Products>();

            products.Remove(products.FirstOrDefault(p => p.Id == pId)!);

            HttpContext.Session.Set("Cart", products);

            return RedirectToAction("Index");
        }
    }
}
