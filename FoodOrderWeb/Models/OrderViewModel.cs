using FoodOrder.Persistance;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace FoodOrderWeb.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(60, ErrorMessage = "A rendelő neve maximum 60 karakter lehet.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;

        public IList<Products> Products { get; set; } = null!;

        public bool Done { get; set; }
    }
}
