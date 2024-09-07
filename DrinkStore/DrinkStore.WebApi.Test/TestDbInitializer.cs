using DrinkStore.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkStore.WebApi.Test
{
    public static class TestDbInitializer
    {

        public static void Initialize(DrinkStoreContext context)
        {
            MainCategory mainCategory = new MainCategory()
            {
                Id = 1,
                Name = "TEST_MAINCATEGORY"
            };

            context.Database.EnsureCreated();

            context.MainCategories.Add(mainCategory);
            context.SaveChanges();

            IList<Category> categories = new List<Category>
            {
                new Category()
                {
                    Id = 1,
                    Name = "TEST_CATEGORY1",
                    MainCategoryId = 1,
                    Products = new List<Product>()
                {
                    new Product()
                    {
                        Id = 1,
                        Manufacturer="TEST_PRODUCT_1",
                        Description="TEST_PRODUCT_1",
                        Price=1,
                        Stock=1,
                        Packaging=Packaging.Piece
                    },
                    new Product()
                    {
                        Id = 2,
                        Manufacturer="TEST_PRODUCT_2",
                        Description="TEST_PRODUCT_2",
                        Price=2,
                        Stock=2,
                        Packaging=Packaging.Piece | Packaging.Tray,
                    }
                    }
                },
                new Category()
                {
                    Id=2,
                    Name = "TEST_CATEGORY2",
                    MainCategoryId = 1,
                    Products = new List<Product>()
                        {
                            new Product()
                            {
                                Id = 3,
                                Manufacturer="TEST_PRODUCT_3",
                                Description="TEST_PRODUCT_3",
                                Price=3,
                                Stock=3,
                                Packaging=Packaging.Piece | Packaging.Shrink
                            },
                    }
                },
                new Category()
                {
                    Id=3,
                    Name = "TEST_CATEGORY3",
                    MainCategoryId = 1,
                    Products = new List<Product>(){ }

                }
            };

            foreach (Category c in categories)
                context.Categories.Add(c);

            context.SaveChanges();

            Order order = new Order
            {
                Id=1,
                Name = "TEST_ORDER1",
                Address = "TEST_ORDER1",
                Email = "TEST_ORDER1",
                PhoneNumber = "11111111111",
                IsDone = false,
                Date = DateTime.Now
            };
            Order order2 = new Order
            {
                Id = 2,
                Name = "TEST_ORDER2",
                Address = "TEST_ORDER2",
                Email = "TEST_ORDER2",
                PhoneNumber = "11111111111",
                IsDone = false,
                Date = DateTime.Now
            };

            context.Orders.Add(order);
            context.Orders.Add(order2);
            context.SaveChanges();

            OrderLine orderLine1 = new OrderLine
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                Packaging = Packaging.Piece
            };
            OrderLine orderLine2 = new OrderLine
            {
                OrderId = 2,
                ProductId = 1,
                Quantity = 1,
                Packaging = Packaging.Piece
            };

            context.OrderLines.Add(orderLine1);
            context.OrderLines.Add(orderLine2);
            context.SaveChanges();

        }
    }
}
