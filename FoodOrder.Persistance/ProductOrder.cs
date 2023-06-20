using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrder.Persistance
{
    public class ProductOrder
    {
        [Key]
        public int ProductOrderId { get; set; }

        [ForeignKey("Orders")]
        public int OrderID { get; set; }

        [ForeignKey("Products")]
        public int ProductID { get; set; }

        public virtual Products Product { get; set; } = null!;
    }
}
