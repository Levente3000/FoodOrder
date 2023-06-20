using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string OrdererName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool Done { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DoneDate { get; set; }
        public int SumPrice { get; set; }
    }
}
