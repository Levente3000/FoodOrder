using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Persistance
{
    public class Employees : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
    }
}
