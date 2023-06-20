using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrder.Persistance
{
    public class DbInitializer
    {
        private static FoodOrderDbContext _context = null!;
        private static UserManager<Employees> _userManager = null!;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<FoodOrderDbContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Employees>>();

            //_context.Database.Migrate();

            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();


            if (_context.Categories.Any())
            {
                return;
            }

            SeedAccounts();
            SeedCategories();
            SeedProducts();
        }

        private static void SeedAccounts()
        {
            if (!_context.Employees.Any())
            {
                var accounts = new Employees[]
                {
                    new Employees { FullName = "Sanyi", UserName = "admin1" },
                    //new Employees { FullName = "Feri", UserName = "admin2" }
                };
                var adminPassword = "Testpw123";
                _userManager.CreateAsync(accounts[0], adminPassword);
                //_userManager.CreateAsync(accounts[1], adminPassword);

                foreach (Employees e in accounts)
                {
                    _context.Employees.Add(e);
                }

                _context.SaveChanges();
            }
        }

        private static void SeedCategories()
        {
            var categories = new Category[]
            {
                new Category {Name = "Levesek", IsDrink = false },
                new Category {Name = "Pizzák", IsDrink = false },
                new Category {Name = "Hamburgerek", IsDrink = false},
                new Category {Name = "Üdítők", IsDrink = true}
            };
            foreach (Category c in categories)
            {
                _context.Categories.Add(c);
            }

            _context.SaveChanges();
        }

        private static void SeedProducts()
        {
            var products = new Products[]
            {
                new Products
                {
                    Name = "Sajtkrémleves",
                    CategoryName = "Levesek",
                    Description = "sajt, tejföl, finomliszt, krémsajt, leveskocka",
                    Price = 400,
                    Spicy = false,
                    Vegetarian = true
                },
                new Products
                {
                    Name = "Húsleves",
                    CategoryName = "Levesek",
                    Description = "hús, olaj vagy zsír, sárgarépa, zellerzöld, fehérrépa, vöröshagyma , karalábé, zeller, csigatészta",
                    Price = 500,
                    Spicy = false,
                    Vegetarian = false
                },
                new Products
                {
                    Name = "Gulyásleves",
                    CategoryName = "Levesek",
                    Description = "hús, olaj vagy zsír, vöröshagyma, zellerzöld, zöldpaprika, paradicsom , burgonya, csipetke",
                    Price = 600,
                    Spicy = true,
                    Vegetarian = false
                },
                new Products
                {
                    Name = "Pepperoni",
                    CategoryName = "Pizzák",
                    Description = "paradicsomos alap, mozzarella, vastagkolbász (allergének: glutén, tej)",
                    Price = 1500,
                    Spicy = false,
                    Vegetarian = false
                },
                new Products
                {
                    Name = "Magyaros",
                    CategoryName = "Pizzák",
                    Description = "paradicsomos alap, mozzarella, gomba, szalámi, hagyma, hegyes erős paprika, csípős (allergének: glutén, tej)",
                    Price = 1800,
                    Spicy = true,
                    Vegetarian = false
                },
                new Products
                {
                    Name = "Brokkolis",
                    CategoryName = "Pizzák",
                    Description = "paradicsomos alap, mozzarella, füstölt mozzarella sajt, brokkoli, rukkola (allergének: glutén, tej)",
                    Price = 1500,
                    Spicy = false,
                    Vegetarian = true
                },
                new Products
                {
                    Name = "Sajtos",
                    CategoryName = "Hamburgerek",
                    Description = "hamburgerbuci, cheddar sajt, majonéz, mustár, ketchup, kígyóuborka, paradicsom, fehérhagyma, jégsaláta",
                    Price = 1000,
                    Spicy = false,
                    Vegetarian = false
                },
                new Products
                {
                    Name = "Vegetáriánus",
                    CategoryName = "Hamburgerek",
                    Description = "hamburgerbuci, füstölt halloumi sajt, aszalt paradicsomos majonéz, grillezett paprika, lila hagyma, rukkola",
                    Price = 1200,
                    Spicy = false,
                    Vegetarian = true
                },
                new Products
                {
                    Name = "BBQ-s",
                    CategoryName = "Hamburgerek",
                    Description = "hamburgerbuci, füstölt sajt, jégsaláta, bacon, jalapeno paprika, karamellizált hagyma, BBQ szósz",
                    Price = 1300,
                    Spicy = true,
                    Vegetarian = false
                },
                new Products
                {
                    Name = "Cola",
                    CategoryName = "Üdítők",
                    Price = 400,
                    Spicy = false,
                    Vegetarian = true
                },
                new Products
                {
                    Name = "Fanta",
                    CategoryName = "Üdítők",
                    Price = 400,
                },
                new Products
                {
                    Name = "Kinley",
                    CategoryName = "Üdítők",
                    Price = 400,
                },
            };

            foreach (Products c in products)
            {
                _context.Products.Add(c);
            }

            _context.SaveChanges();
        }
    }
}
