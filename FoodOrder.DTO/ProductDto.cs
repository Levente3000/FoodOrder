using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public int Price { get; set; }
        public bool Spicy { get; set; }
        public bool Vegetarian { get; set; }
    }
}
