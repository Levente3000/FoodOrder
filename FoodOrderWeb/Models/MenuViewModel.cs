using FoodOrder.Persistance;

namespace FoodOrderWeb.Models
{
    public class MenuViewModel
    {
        public IList<Category>? Categories { get; set; } = null!;
        public IList<Products>? Products { get; set; } = null!;
        public IList<Products>? Top10List { get; set; } = null!;
    }
}
