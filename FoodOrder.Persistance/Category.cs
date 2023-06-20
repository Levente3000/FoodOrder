using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Persistance
{
    public class Category
    {
        [Key]
        public string Name { get; set; } = null!;

        public bool IsDrink { get; set; }
    }
}
