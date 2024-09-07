using DrinkStore.Persistence;
using DrinkStore.Persistence.DTO;
using DrinkStore.Persistence.Services;
using DrinkStore.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DrinkStore.WebApi.Test
{
    public class DrinkStoreTest : IDisposable
    {
        private readonly DrinkStoreContext _context;
        private readonly DrinkStoreService _service;

        //Kontrollerek
        private readonly ProductsController _productsController;
        private readonly CategoriesController _categoriesController;
        private readonly MainCategoriesController _mainCategoriesController;
        private readonly OrdersController _ordersController;
        public DrinkStoreTest()
        {
            var options = new DbContextOptionsBuilder<DrinkStoreContext>()
                .UseInMemoryDatabase("DrinkStoreTest")
                .Options;

           
            _context = new DrinkStoreContext(options);
            TestDbInitializer.Initialize(_context);
            _service = new DrinkStoreService(_context);

            _productsController = new ProductsController(_service);
            _categoriesController = new CategoriesController(_service);
            _mainCategoriesController = new MainCategoriesController(_service);
            _ordersController = new OrdersController(_service);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        //Termékek
        #region Products

        //Létező termék lekérdezése
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetProductByIdTest(Int32 Id)
        {
            // Act
            var result = _productsController.GetProductById(Id);

            // Assert
            var content = Assert.IsAssignableFrom<ProductDto>(result.Value);
            Assert.Equal(Id,content.Id);
        }

        //Nem létező termék
        [Fact]
        public void GetInvalidProductByIdTest()
        {
            // Arrange
            var id = 4;

            // Act
            var result = _productsController.GetProductById(id);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        //Sikerült létrehozni a terméket
        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        public void PostProductTest(Int32 Id)
        {
            //Arrange
            ProductDto productDto = new ProductDto()
            {
                Id = Id,
                Manufacturer = "TEST_PRODUCT",
                Description = "TEST_PRODUCT",
                CategoryId = 1,
                Stock = 1,
                Packagings = PackagingDto.Convert(Packaging.Piece)
            };

            //Act
            var result = _productsController.PostProduct(productDto);

            //Assert
            var objectResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result.Result);
            var content = Assert.IsAssignableFrom<ProductDto>(objectResult.Value);

            Assert.Equal(productDto.Id, content.Id);
            Assert.Equal(productDto.Manufacturer, content.Manufacturer);
            Assert.Equal(productDto.Description, content.Description);
            Assert.Equal(productDto.CategoryId, content.CategoryId);
            Assert.Equal(productDto.Stock, content.Stock);
            Assert.Equal(PackagingDto.Convert(productDto.Packagings), PackagingDto.Convert(content.Packagings));
        }

        //Nem sikerült létrehozni a terméket
        [Theory]
        [InlineData(1)]
        public void PostInvalidProductTest(Int32 Id)
        {
            //Arrange
            ProductDto productDto = new ProductDto()
            {
                Id = Id,
                Manufacturer = "TEST_PRODUCT",
                Description = "TEST_PRODUCT",
                CategoryId = 1,
                Stock = 1,
                Packagings = PackagingDto.Convert(Packaging.Piece)
            };

            //Act
            var result = _productsController.PostProduct(productDto);

            // Assert
            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result.Result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status500InternalServerError);
        }

        //Termék módosítása
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void PutProductTest(Int32 Id)
        {
            //Arrange
            ProductDto productDto = new ProductDto()
            {
                CategoryId = 1,
                Manufacturer = "TEST_PRODUCT",
                Description = "TEST_PRODUCT",
                Stock = 1,
                Packagings = PackagingDto.Convert(Packaging.Piece)
            };

            //Act
            var result = _productsController.PutProduct(Id,productDto);

            // Assert

            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status200OK);
        }

        //Két ID nem egyezik
        [Theory]
        [InlineData(10)]
        public void PutInvalidProductIdTest(Int32 Id)
        {
            //Arrange
            ProductDto productDto = new ProductDto()
            {
                Id = 3,
                Manufacturer = "TEST_PRODUCT",
                Description = "TEST_PRODUCT",
                CategoryId = 1,
                Stock = 1,
                Packagings = PackagingDto.Convert(Packaging.Piece)
            };

            //Act
            var result = _productsController.PutProduct(Id, productDto);

            // Assert
            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status400BadRequest);
        }

        //Adatbázis hiba
        [Theory]
        [InlineData(3)]
        public void PutInvalidProductDbTest(Int32 Id)
        {
            //Arrange
            ProductDto productDto = new ProductDto()
            {
                Id = 3,
                Manufacturer = "TEST_PRODUCT",
                Description = "TEST_PRODUCT",
                CategoryId = 1,
                Stock = 1,
                Packagings = PackagingDto.Convert(Packaging.Piece)
            };

            //Act
            var result = _productsController.PutProduct(Id, productDto);

            // Assert
            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status500InternalServerError);
        }

        #endregion

        //Kategóriák
        #region Categories

        //Létező kategória
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetCategoryByIdTest(Int32 Id)
        {
            // Act
            var result = _categoriesController.GetCategoryById(Id);
            var content = _context.Categories.Where(c => c.Id == Id).Include("Products").Single().Products.ToList();

            // Assert
            var value = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(result.Value);
            Assert.Equal(value.ToList().Count, content.Count);
            Assert.Equal(value.ElementAt(0).Id, content.ElementAt(0).Id);
        }

        //Nem létező kategória
        [Fact]
        public void GetInvalidCategoryByIdTest()
        {
            // Arrange
            var id = 99;

            // Act
            var result = _categoriesController.GetCategoryById(id);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }


        #endregion

        //Főkategóriák
        #region MainCategories


        //Összes főkategória lekérdezése
        [Fact]
        public void GetMainCategoriesTest()
        {
            // Act
            var result = _mainCategoriesController.GetMainCategories();
            var content = _context.MainCategories.Select(mc => (MainCategoryDto)mc).ToList();

            // Assert
            var value = Assert.IsAssignableFrom<IEnumerable<MainCategoryDto>>(result.Value);
            Assert.Equal(value.ToList().Count, content.Count);
            Assert.Equal(value.ElementAt(0).Id, content.ElementAt(0).Id);
        }


        //Létező főkategória alapján lekérdezés
        [Fact]
        public void GetCategoriesByMainCategoryTest()
        {
            // Arrange
            int Id = 1;
            // Act
            var result = _mainCategoriesController.GetCategoriesByMainCategory(Id);
            var content = _context.Categories.OrderBy(c => c.Name).Where(c => c.MainCategoryId == Id).ToList();

            // Assert
            var value = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(result.Value);
            Assert.Equal(value.ToList().Count, content.Count);
            Assert.Equal(value.ElementAt(0).Id, content.ElementAt(0).Id);
        }

        //Nem létező főkategória alapján lekérdezés
        [Fact]
        public void GetCategoriesByInvalidMainCategoryTest()
        {
            // Arrange
            int Id = 2;
            // Act
            var result = _mainCategoriesController.GetCategoriesByMainCategory(Id);
            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }
        #endregion

        //Rendelések
        #region Orders

        //Rendelések lekérése
        [Fact]
        public void GetOrdersTest()
        {
            // Arrange
            var content = _context.Orders.ToList();

            // Act
            var result = _ordersController.GetOrders();

            // Assert
            var value = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(result.Value);
            Assert.Equal(value.ToList().Count, content.Count);
            Assert.Equal(value.ElementAt(0).Id, content.ElementAt(0).Id);
        }

        //Rendeléshez tartozó sorok lekérése
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetOrderLinesByOrderIdTest(int id)
        {
            // Arrange
            var content = _context.OrderLines.Where(ol => ol.OrderId == id).ToList();

            // Act
            var result = _ordersController.GetOrderLinesByOrderId(id);

            // Assert
            var value = Assert.IsAssignableFrom<IEnumerable<OrderLineDto>>(result.Value);
            Assert.Equal(value.ToList().Count, content.Count);
            Assert.Equal(value.ElementAt(0).OrderId, content.ElementAt(0).OrderId);
            Assert.Equal(value.ElementAt(0).ProductId, content.ElementAt(0).ProductId);
            Assert.Equal(value.ElementAt(0).Quantity, content.ElementAt(0).Quantity);
        }

        //Nem létező rendeléshez tartozó sorok lekérése
        [Fact]
        public void GetOrderLinesByInvalidOrderIdTest()
        {
            // Arrange
            var id = 3;
            // Act
            var result = _ordersController.GetOrderLinesByOrderId(id);
            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
        }

        //Szűrt Rendelések lekérése
        [Fact]
        public void GetFilteredOrdersTest()
        {
            // Arrange
            FilterDto f = new FilterDto()
            {
                Name = "TEST",
                Done = true,
                NotDone = true,
                FromDate = DateTime.Now.AddDays(-1),
                ToDate = DateTime.Now.AddDays(1)
            };

            var content =   _context.Orders.Where(o =>
                            o.Date >= f.FromDate &&
                            o.Date < f.ToDate &&
                            o.Name.ToLower().Contains(f.Name.ToLower())).ToList();

            // Act
            var result = _ordersController.GetFilteredOrders(f);

            // Assert
            var value = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(result.Value);
            Assert.Equal(value.ToList().Count, content.Count);
        }

        //Rendelés módosítása
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void PutOrderTest(Int32 Id)
        {
            //Arrange
            OrderDto orderDto = new OrderDto()
            {
                Name = "TEST",
                Address = "TEST",
                Email = "TEST",
                PhoneNumber = "TEST",
                IsDone = true
            };

            //Act
            var result = _ordersController.PutOrder(Id, orderDto);

            // Assert

            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status200OK);
        }

        //Két ID nem egyezik
        [Theory]
        [InlineData(10)]
        public void PutInvalidOrderIdTest(Int32 Id)
        {
            //Arrange
            OrderDto orderDto = new OrderDto()
            {
                Id=2,
                Name = "TEST",
                Address = "TEST",
                Email = "TEST",
                PhoneNumber = "TEST",
                IsDone = true
            };

            //Act
            var result = _ordersController.PutOrder(Id, orderDto);

            // Assert
            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status400BadRequest);
        }

        //Adatbázis hiba
        [Theory]
        [InlineData(1)]
        public void PutInvalidOrderDbTest(Int32 Id)
        {
            //Arrange
            OrderDto orderDto = new OrderDto()
            {
                Id=1,
                Name = "TEST",
                Address = "TEST",
                Email = "TEST",
                PhoneNumber = "TEST",
                IsDone = true
            };
            //Act
            var result = _ordersController.PutOrder(Id, orderDto);

            // Assert
            var objectResult = Assert.IsAssignableFrom<StatusCodeResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status500InternalServerError);
        }
        #endregion

    }
}
