using FoodOrder.Persistance;

namespace FoodOrderWeb.Models
{
    public class ProductsViewModel
    {
        public string? CategoryName { get; set; } = null!;
        public IList<Products>? Products { get; set; } = null!;
    }
}
