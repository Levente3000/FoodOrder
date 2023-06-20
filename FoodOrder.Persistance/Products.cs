using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrder.Persistance
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        [ForeignKey("Category")]
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public int Price { get; set; }
        public bool Spicy { get; set; }
        public bool Vegetarian { get; set; }
    }
}
