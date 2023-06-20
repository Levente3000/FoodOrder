using FoodOrder.Persistance;
using FoodOrder.Persistance.Services;
using FoodOrderWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderWeb.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IFoodOrderService _foodOrderService;

        public HomeController(IFoodOrderService foodservice)
        {
            _foodOrderService = foodservice;
        }

        public IActionResult Index()
        {
            MenuViewModel viewModel = new MenuViewModel();
            viewModel.Categories = _foodOrderService.GetAllCategories();
            viewModel.Products = _foodOrderService.GetTop10Products();
            return View("Index", viewModel);
        }

        public IActionResult ProductList(string cn)
        {
            ProductsViewModel viewModel = new ProductsViewModel();
            viewModel.CategoryName = cn;
            viewModel.Products = _foodOrderService.GetAllProducts(cn).ToList();

            if (viewModel.Products.Count == 0)
                return RedirectToAction(nameof(ProductList));

            return View("Details", viewModel);
        }

        [HttpPost]
        public IActionResult ProductListFiltered(string cn, string SearchString)
        {
            ProductsViewModel viewModel = new ProductsViewModel();
            viewModel.CategoryName = cn;
            IList<Products> products = _foodOrderService.GetAllProducts(viewModel.CategoryName).ToList();

            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower();
                viewModel.Products = new List<Products>();

                viewModel.Products = products.Where(p => p.Name.ToLower().Contains(SearchString)).ToList();
            }

            return View("Details", viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}