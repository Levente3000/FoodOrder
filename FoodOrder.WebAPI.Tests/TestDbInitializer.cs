using FoodOrder.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.WebAPI.Tests
{
    public static class TestDbInitializer
    {
        public static void Initialize(FoodOrderDbContext context, ref Mock<SignInManager<Employees>>? SignInManagerMock)
        {
            Mock<UserManager<Employees>>? UserManagerMock;

            var userStore = new Mock<IUserStore<Employees>>();
            UserManagerMock = new(userStore.Object, null, null, null, null, null, null, null, null);

            UserManagerMock.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Employees { UserName = "admin1" });
            UserManagerMock.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new Employees { UserName = "admin" });

            SignInManagerMock = new(UserManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<Employees>>(), null, null, null, null);

            SignInManagerMock.Setup(signInManager => signInManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(new SignInResult());

            SignInManagerMock.Setup(signInManager => signInManager.SignOutAsync());

            SeedAccounts(context);
            SeedCategories(context);
            SeedProducts(context);
            SeedOrders(context);
        }

        private static void SeedAccounts(FoodOrderDbContext context)
        {
            if (!context.Employees.Any())
            {
                var accounts = new Employees[]
                {
                    new Employees { FullName = "Sanyi", UserName = "admin1" },
                };
                var adminPassword = "Testpw123";

                foreach (Employees e in accounts)
                {
                    context.Employees.Add(e);
                }

                context.SaveChanges();
            }
        }

        private static void SeedCategories(FoodOrderDbContext context)
        {
            var categories = new Category[]
            {
                new Category {Name = "Levesek"},
                new Category {Name = "Pizzák"},
                new Category {Name = "Hamburgerek"},
                new Category {Name = "Üdítők"}
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }

            context.SaveChanges();
        }

        private static void SeedProducts(FoodOrderDbContext context)
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
                context.Products.Add(c);
            }

            context.SaveChanges();
        }

        private static void SeedOrders(FoodOrderDbContext context)
        {
            var orders = new Orders[]
            {
                new Orders
                {
                    OrderId = 1,
                    OrdererName = "Sanyi",
                    PhoneNumber = "06300000001",
                    Address = "Hunyadi u. 31",
                    Done = false,
                    DoneDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                },
                new Orders
                {
                    OrderId = 2,
                    OrdererName = "Feri",
                    PhoneNumber = "06300000002",
                    Address = "Hunyadi u. 32",
                    Done = false,
                    DoneDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                },
            };

            var productOrders = new ProductOrder[]
            {
                new ProductOrder
                {
                    ProductOrderId = 1,
                    OrderID = 1,
                    ProductID = 1,
                },
                new ProductOrder
                {
                    ProductOrderId = 2,
                    OrderID = 1,
                    ProductID = 7,
                },
                new ProductOrder
                {
                    ProductOrderId = 3,
                    OrderID = 1,
                    ProductID = 5,
                },
                new ProductOrder
                {
                    ProductOrderId = 4,
                    OrderID = 2,
                    ProductID = 1,
                },
                new ProductOrder
                {
                    ProductOrderId = 5,
                    OrderID = 2,
                    ProductID = 2,
                },
                new ProductOrder
                {
                    ProductOrderId = 6,
                    OrderID = 2,
                    ProductID = 3,
                },
                new ProductOrder
                {
                    ProductOrderId = 7,
                    OrderID = 2,
                    ProductID = 3,
                },
            };

            foreach (Orders c in orders)
            {
                context.Orders.Add(c);
            }

            foreach (ProductOrder c in productOrders)
            {
                context.ProductOrders.Add(c);
            }

            context.SaveChanges();
        }
    }
}
