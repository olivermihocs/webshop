using DrinkStore.Persistence.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinkStore.Persistence.Services
{
    public class DrinkStoreService
    {
        private readonly DrinkStoreContext _context;

        private readonly Dictionary<Packaging, String> _dict; //Könyvtár a csomagolások tárolására

        public DrinkStoreService(DrinkStoreContext context)
        {
            _context = context;

            _dict = new Dictionary<Packaging, string>() { //Elérhető csomagolások
                {Packaging.Piece, "Darab" },
                {Packaging.Shrink, "Zsugorfólia" },
                {Packaging.Bin, "Rekesz" },
                {Packaging.Tray, "Tálca" }
            };
        }

        #region Category

        //Visszaadja a főkategóriához tartozó alkategóriákat
        public List<MainCategory> GetMainCategories()
        {
            return _context.MainCategories.OrderBy(c => c.Name).ToList();
        }

        //Visszaadja a főkategóriához tartozó alkategóriákat
        public List<Category> GetCategoriesByMainCategory(Int32 McId)
        {
            if (IsExistingMainCategory(McId))
            {
                return _context.Categories.OrderBy(c => c.Name).Where(c => c.MainCategoryId == McId).ToList();
            }
            else
            {
                return null;
            }
        }

        private Boolean IsExistingMainCategory(Int32 Id)
        {
            return _context.MainCategories.Where(mc => mc.Id == Id).ToList().Count > 0;
        }

        //Visszaadja egy kategória nevét és a hozzá tartozó termékek listáját, figyelve a rendezésre/oldalszámra
        public Tuple<string, IEnumerable<Product>> GetCategoryById(int categoryId,int n,bool orderByManufacturer, bool orderByPrice, bool reverse)
        {

            Category category = _context.Categories.Where(c => c.Id == categoryId).Include("Products").FirstOrDefault();

            if (category == null)
            {
                return null;
            }

            IEnumerable<Product> products = category.Products;
            if (orderByManufacturer) //Gyártó szerinti rendezés
            {
                if (reverse)
                {
                    products = products.OrderByDescending(p => p.Manufacturer);
                }
                else
                {
                    products = products.OrderBy(p => p.Manufacturer);
                }

            }
            else if (orderByPrice) //Ár szerinti rendezés
            {
                if (reverse)
                {
                    products = products.OrderByDescending(p => p.Price);
                }
                else
                {
                    products = products.OrderBy(p => p.Price);
                }
            }

            return new Tuple<string, IEnumerable<Product>>(category.Name, products.Skip((n - 1) * 20).Take(21));

        }
        #endregion

        #region Product

        //Termék azonosíó alapján
        public Product GetProductById(Int32 ProductId)
        {
            return _context.Products.Where(p => p.Id == ProductId).Single();
        }

        #endregion

        #region Cart

        //Van-e készleten az adott termékből, az inCart paramatérrel megadható hogy hány van jelenleg a kosárban.
        public bool ValidateStock(int productId, int quantity, Packaging packaging, int inCart=0)
        {
            Product product = null;
            try
            {
                product = GetProductById(productId);
            }
            catch
            {
                return false;
            }

            return quantity* GetValueOfPackaging(packaging) + inCart*GetValueOfPackaging(packaging) <= product.Stock;
        }

        //Csomagolás neve érték alapján
        public String GetNameOfPackaging(Packaging packaging)
        {
            return _dict[packaging];
        }

        //Csomagolás értéke név alapján
        public Packaging GetPackagingByName(String packagingStr)
        {
            return _dict.FirstOrDefault(x => x.Value == packagingStr).Key;
        }

        public List<String> GetPackagingList(Packaging packaging)
        {
            List<String> pList = new List<String>();
            if ((packaging & Packaging.Piece) > 0)
            {
                pList.Add(GetNameOfPackaging(Packaging.Piece));
            }
            if ((packaging & Packaging.Bin)>0)
            {
                pList.Add(GetNameOfPackaging(Packaging.Bin));
            }
            if ((packaging & Packaging.Tray) > 0)
            {
                pList.Add(GetNameOfPackaging(Packaging.Tray));
            }
            if ((packaging & Packaging.Shrink) > 0)
            {
                pList.Add(GetNameOfPackaging(Packaging.Shrink));
            }
            return pList;
        }

        public int GetValueOfPackaging(Packaging packaging)
        {
            switch (packaging)
            {
                case Packaging.Piece: //Darab
                    return 1;
                case Packaging.Shrink: //Zsugorfólia
                    return 6;
                case Packaging.Bin: //Rekesz
                    return 12;
                case Packaging Tray: //Tálca
                    return 24;
            }
        }

        //ÁFA,(azért fv. mert az alkalmazás fejlesztésével a jővőben esetleg egyéni adókat rakhatunk az egyes termékekre/kategóriákra.)
        public int GetVAT()
        {
            return 27;
        }

        //Egy rendelés felvétele az adatbázisba.
        public bool CreateOrder(string Name, string Address, string Email, string PhoneNumber,List<CartItem> cartItems)
        {
            if (cartItems==null || cartItems.Count() == 0)
            {
                return false;
            }

            //Rendelés felvétele
            Order order = new Order
            {
                Name = Name,
                Address = Address,
                Email = Email,
                PhoneNumber = PhoneNumber,
                IsDone = false,
                Date = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            //A rendeléshez tartozó sorok felvétele
            foreach(CartItem item in cartItems)
            {
                OrderLine orderLine = new OrderLine();
                orderLine.OrderId = order.Id;
                orderLine.ProductId = item.ProductId;
                orderLine.Quantity = item.Quantity;
                orderLine.Packaging = item.Packaging;
                _context.OrderLines.Add(orderLine);
            }
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return false;
            }
            return true;
        }

        #endregion


        #region WebApi
   
        public List<Product> GetCategoryById(int categoryId)
        {
            var c = _context.Categories.Where(c => c.Id == categoryId).Include("Products").Single();
            return c.Products.ToList();
        }

        public Product CreateProduct(Product product)
        {
            try
            {
                _context.Add(product);
                _context.SaveChanges();
            }
            catch
            {
                return null;
            }
            return product;
        }

        public bool UpdateProduct(Product product)
        {
            try
            {
                _context.Update(product);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }
        public List<OrderLine> GetOrderLinesByOrderId(int id)
        {
            if (IsExistingOrder(id))
                return _context.OrderLines.Where(ol => ol.OrderId == id).ToList();
            else
                return null;
        }

        private Boolean IsExistingOrder(Int32 Id)
        {
            return _context.Orders.Where(o => o.Id == Id).ToList().Count > 0;
        }

        public List<Order> GetFilteredOrders(FilterDto filter)
        {
            if(!filter.Done && !filter.NotDone)
            {
                return new List<Order>();
            }

            var result = _context.Orders.Where(o =>
                                       o.Date >= filter.FromDate.Date &&
                                       o.Date < filter.ToDate.Date.AddDays(1) &&
                                       o.Name.ToLower().Contains(filter.Name.ToLower()));
            if (!filter.Done)
            {
                result = result.Where(o => !o.IsDone);
            }
            if (!filter.NotDone)
            {
                result = result.Where(o => o.IsDone);
            }

            return result.ToList();
        }

        public bool UpdateOrder(Int32 Id,Order order) 
        {
            try
            {
                Order current = _context.Orders.Where(o => o.Id == Id).First();
                if (current.IsDone != order.IsDone) //Már teljesítve van-e a kérés
                {
                    return true;
                }

                bool update = true;
                List <OrderLine> orderLines = GetOrderLinesByOrderId(Id);

                //Termékek elérhetőek-e a megadott mennyiségben megerősítés esetén
                if (!current.IsDone)
                {
                    foreach (OrderLine line in orderLines)
                    {
                        update &= ValidateStock(line.ProductId, line.Quantity, line.Packaging);
                    }
                }
                
                if (!update)
                    return false;

                foreach (OrderLine line in orderLines)
                {
                    Product product = GetProductById(line.ProductId);
                    if (!current.IsDone) //Rendelés megerősítése
                    {
                        product.Stock -= line.Quantity * GetValueOfPackaging(line.Packaging);
                    }
                    else //Rendelés visszavonása
                    {
                        product.Stock += line.Quantity * GetValueOfPackaging(line.Packaging);
                    }
                    _context.Update(product);
                }

                current.IsDone = !current.IsDone;
                _context.Update(current);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
    }
}
#endregion