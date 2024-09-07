using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkStore.Persistence
{
    public class DbInitializer
    {
        private static DrinkStoreContext _context;
        private static UserManager<Employee> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<DrinkStoreContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

           _context.Database.Migrate();

            //Adatbázis inicializálása
            if (!_context.Categories.Any())
            {
                SeedDb();
            }

            // Felhasználók inicializálása
            if (!_context.Users.Any())
            {
                SeedUsers();
            }

        }

        //Felhasználók hozzáadása.
        private static void SeedUsers()
        {
            var adminUser = new Employee
            {
                UserName = "admin",
                FullName = "Admin bacsi"
            };

            var adminPassword = "Admin123";
            var adminRole = new IdentityRole<int>("administrator");

            var result1 = _userManager.CreateAsync(adminUser, adminPassword).Result;
            var result2 = _roleManager.CreateAsync(adminRole).Result;
            var result3 = _userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;

        }

        //Adatbázis feltöltése adatokkal a teszteléshez.
        private static void SeedDb()
        {
            MainCategory Alcoholic = new MainCategory()
            {
                Name = "Alkoholos"
            };
            MainCategory nonAlcoholic = new MainCategory()
            {
                Name = "Alkoholmentes"
            };
            _context.MainCategories.Add(Alcoholic);
            _context.MainCategories.Add(nonAlcoholic);
            _context.SaveChanges();

            Random rnd = new Random();

            IList<Category> categories = new List<Category>
            {
                new Category()
                {
                    Name = "Üditőitalok",
                    MainCategoryId = nonAlcoholic.Id,
                    Products = new List<Product>()
                {
                    new Product()
                    {
                        Manufacturer="Nestea",
                        Description="citrom izű tea 1,5L",
                        Price=320,
                        Stock=125,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Tray | Packaging.Bin,
                    },
                    new Product()
                    {
                        Manufacturer="Nestea",
                        Description="barack izű tea 1,5L",
                        Price=320,
                        Stock=100,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Tray,
                    },
                    new Product()
                    {
                        Manufacturer="Nestea",
                        Description="citrom izű tea 0,5L",
                        Price=195,
                        Stock=105,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink,
                    },
                    new Product()
                    {
                        Manufacturer="Nestea",
                        Description="barack izű tea 0,5L",
                        Price=195,
                        Stock=55,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink,
                    },
                    new Product()
                    {
                        Manufacturer="Lipton",
                        Description="Green Ice Tea 1,5L",
                        Price=320,
                        Stock=126,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Lipton",
                        Description="Green Ice Tea 0,5L",
                        Price=195,
                        Stock=56,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Coca-Cola",
                        Description="0,5L",
                        Price=205,
                        Stock=70,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Tray
                    },
                    new Product()
                    {
                        Manufacturer="Coca-Cola",
                        Description="0,75L",
                        Price=225,
                        Stock=101,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="Coca-Cola",
                        Description="1,25L",
                        Price=255,
                        Stock=210,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Tray
                    },
                    new Product()
                    {
                        Manufacturer="Coca-Cola",
                        Description="Zero 0,5L",
                        Price=200,
                        Stock=346,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Coca-Cola",
                        Description="Zero 0,75L",
                        Price=215,
                        Stock=106,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Coca-Cola",
                        Description="Zero 1,25L",
                        Price=245,
                        Stock=56,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="Sprite",
                        Description="citrom és lime 0,5L",
                        Price=205,
                        Stock=175,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Sprite",
                        Description="citrom és lime 0,75L",
                        Price=220,
                        Stock=189,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Sprite",
                        Description="citrom és lime 1,25L",
                        Price=255,
                        Stock=75,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="Sprite",
                        Description="citrom és lime 1,5L",
                        Price=285,
                        Stock=25,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="Fanta",
                        Description="narancs 0,5L",
                        Price=185,
                        Stock=142,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Fanta",
                        Description="narancs 0,75L",
                        Price=225,
                        Stock=100,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Bin
                    },
                    new Product()
                    {
                        Manufacturer="Fanta",
                        Description="narancs 1,25L",
                        Price=275,
                        Stock=32,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="Fanta",
                        Description="narancs 1,5L",
                        Price=295,
                        Stock=125,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="HELL",
                        Description="Classic 250ml",
                        Price=185,
                        Stock=125,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    },
                    new Product()
                    {
                        Manufacturer="HELL",
                        Description="Summer cool 250ml",
                        Price=190,
                        Stock=125,
                        TypeNo=rnd.Next().ToString(),
                        Packaging=Packaging.Piece | Packaging.Shrink
                    }
                }
                },

                new Category()
                {
                    Name = "Gyümölcslevek",
                    MainCategoryId = nonAlcoholic.Id,
                    Products = new List<Product>()
                        {
                            new Product()
                            {
                                Manufacturer="Cappy",
                                Description="Ice fruit alma-körte vegyesgyümölcs ital 1,5l",
                                Price=350,
                                Stock=50,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                            new Product()
                            {
                                Manufacturer="Cappy",
                                Description="Sárgabarack ital 1l",
                                Price=370,
                                Stock=544,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Bin
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-eper-egres 1,5l",
                                Price=300,
                                Stock=106,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Bin
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-eper-egres 0,5l",
                                Price=169,
                                Stock=200,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-meggy 0,75l",
                                Price=185,
                                Stock=23,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-meggy 1,5l",
                                Price=300,
                                Stock=245,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Tray
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-körte 0,75l",
                                Price=185,
                                Stock=23,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-körte 1,5l",
                                Price=670,
                                Stock=245,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Bin
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-őszibarack 0,75l",
                                Price=185,
                                Stock=23,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-őszibarack 1,5l",
                                Price=300,
                                Stock=245,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Tray
                            },
                            new Product()
                            {
                                Manufacturer="Topjoy",
                                Description="Alma-őszibarack 0,5l",
                                Price=169,
                                Stock=245,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Tray
                            }
                        }
                },


                new Category()
                {
                    Name = "Whiskyk",
                    MainCategoryId = Alcoholic.Id,
                    Products = new List<Product>()
                        {
                            new Product()
                            {
                                Manufacturer="Aberfeldy",
                                Description="12 years gold bar edition",
                                Price=7500,
                                Stock=523,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                            new Product()
                            {
                                Manufacturer="Aberfeldy",
                                Description="21 Years whisky",
                                Price=14000,
                                Stock=553,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Tray
                            }
                        }
                },

                new Category()
                {
                    Name = "Vodkák",
                    MainCategoryId = Alcoholic.Id,
                    Products = new List<Product>()
                        {
                            new Product()
                            {
                                Manufacturer="Absolut",
                                Description="Blue vodka 1L",
                                Price=12300,
                                Stock=100,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Bin
                            },
                            new Product()
                            {
                                Manufacturer="Absolut",
                                Description="Blue vodka 4,5L",
                                Price=36500,
                                Stock=103,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece | Packaging.Shrink
                            }
                        }
                }
            };
            ;

            categories.Add(
                    new Category()
                    {
                        Name = "Pezsgők",
                        MainCategoryId = Alcoholic.Id,
                        Products = new List<Product>()
                        {
                            new Product()
                            {
                                Manufacturer="BB",
                                Description="Alkoholmentes 0,75L",
                                Price=989,
                                Stock=100,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece |Packaging.Bin
                            },
                            new Product()
                            {
                                Manufacturer="BB",
                                Description="Arany Cuvée 0,75L",
                                Price=1290,
                                Stock=103,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece |Packaging.Bin
                            },
                            new Product()
                            {
                                Manufacturer="Billecart-Salmon",
                                Description="Brut Vintage 0,75L",
                                Price=22290,
                                Stock=103,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece |Packaging.Bin
                            }
                        }
                    }); ;
            categories.Add(
                    new Category()
                    {
                        Name = "Sör",
                        MainCategoryId = Alcoholic.Id,
                        Products = new List<Product>()
                        {
                            new Product()
                            {
                                Manufacturer="Arany ászok",
                                Description="Dobozos 0,5L",
                                Price=269,
                                Stock=100,
                                TypeNo=rnd.Next().ToString(),
                                Packaging=Packaging.Piece |Packaging.Tray
                            }
                        }
                    }); ;

            foreach (Category Category in categories)
                _context.Categories.Add(Category);

            _context.SaveChanges();

            Order order = new Order
            {
                Name = "Kovács Feri",
                Address = "Petofi utca 2",
                Email = "admin@gmail.com",
                PhoneNumber = "06301231122",
                IsDone = true,
                Date = DateTime.Now
            };

            Order order2 = new Order
            {
                Name = "Nagy Pista",
                Address = "Kossuth utca 3",
                Email = "narancs@citromail.hu",
                PhoneNumber = "06301232211",
                IsDone = false,
                Date = DateTime.Now
            };

            Order order3 = new Order
            {
                Name = "Kiss Jóska",
                Address = "Arany Janos utca 2",
                Email = "Email@cim.hu",
                PhoneNumber = "06301232599",
                IsDone = false,
                Date = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.Orders.Add(order2);
            _context.Orders.Add(order3);
            _context.SaveChanges();

            IList<OrderLine> orderLines = new List<OrderLine>()
            {
                new OrderLine()
                {
                OrderId = order.Id,
                ProductId = 1,
                Quantity = 10,
                Packaging = Packaging.Piece
                },
                new OrderLine()
                {
                OrderId = order.Id,
                ProductId = 2,
                Quantity = 1,
                Packaging = Packaging.Tray
                },
                new OrderLine()
                {
                OrderId = order.Id,
                ProductId = 3,
                Quantity = 2,
                Packaging = Packaging.Bin
                },
                new OrderLine()
                {
                OrderId = order.Id,
                ProductId = 4,
                Quantity = 1,
                Packaging = Packaging.Bin
                },
                new OrderLine()
                {
                OrderId = order2.Id,
                ProductId = 5,
                Quantity = 1,
                Packaging = Packaging.Shrink
                },
                new OrderLine()
                {
                OrderId = order3.Id,
                ProductId = 6,
                Quantity = 2,
                Packaging = Packaging.Piece
                },
                new OrderLine()
                {
                OrderId = order3.Id,
                ProductId = 7,
                Quantity = 2,
                Packaging = Packaging.Bin
                }
            };

            foreach(OrderLine orderLine in orderLines)
            {
                _context.OrderLines.Add(orderLine);
            }
            _context.SaveChanges();


        }
    }
}