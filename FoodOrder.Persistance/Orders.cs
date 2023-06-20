using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Persistance
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public string OrdererName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public virtual ICollection<Products> Products { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool Done { get; set; }
        public int SumPrice { get; set; }
    }
}
