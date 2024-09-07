using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Web.Models;
using DrinkStore.Persistence;
using DrinkStore.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DrinkStore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly DrinkStoreService _service;

        public CartController(DrinkStoreService service)
        {
            _service = service;
        }

        //Kosár lekérése
        [HttpGet]
        public IActionResult Index()
        {

            CartViewModel cartViewModel = BuildCartViewModel(null);

            return View(cartViewModel);
        }

        //Rendelés leadása
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CartViewModel cartViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            CartViewModel newModel = BuildCartViewModel(cartViewModel);

            if (newModel.CartProducts.Count() == 0)
            {
                return View("Index");
            }

            bool created = _service.CreateOrder(newModel.Name,newModel.Address,newModel.Email,newModel.PhoneNumber, GetCartItems());

            if (created)
            {
                ClearSessionCartVar();
                return View("Result", cartViewModel); //Sikeres rendelés
            }

            //Sikertelen rendelés
            ViewBag.ErrorMessage = "A rendelést nem tudtuk feldolgozni, kérjük próbálja később.";
            return View("Index", newModel);

        }

        //Adott munkamenethez tartozó termékek lekérése
        private List<CartItem> GetCartItems()
        {
            var str = HttpContext.Session.GetString("Cart");
            List<CartItem> cartItems;
            if (str == null) //Ha nincs kosár
            {
                return null;
            }
            try
            {
                cartItems = JsonConvert.DeserializeObject<List<CartItem>>(str);
                return cartItems;
            }
            catch
            {
                return null;
            }  
        }


        //Nézetmodell előállítása
        private CartViewModel BuildCartViewModel(CartViewModel model)
        {
            
            List<CartItem> cartItems = GetCartItems();

            List<Tuple<Product, int, int, String>> cartProducts = new List<Tuple<Product, int, int, String>>(); //Termék, mennyiség, teljes mennyiség ,csomagolás
            double price = 0;

            if (cartItems != null)
            {
                foreach (CartItem cartItem in cartItems)
                {
                    Product product = null;
                    try
                    {
                        product = _service.GetProductById(cartItem.ProductId);
                    }
                    catch
                    {

                    }
                    if (product != null && _service.ValidateStock(product.Id, cartItem.Quantity, cartItem.Packaging)) //Megnézzük hogy valid-e a termék és van-e elég készleten
                    {
                        int totalQuantity = cartItem.Quantity * _service.GetValueOfPackaging(cartItem.Packaging);
                        cartProducts.Add(new Tuple<Product, int, int, String>(product, cartItem.Quantity, totalQuantity, _service.GetNameOfPackaging(cartItem.Packaging)));
                        price += product.Price * totalQuantity;
                    }
                }
            }

            if (model == null)
            {
                model = new CartViewModel();
            }
            model.CartProducts = cartProducts;
            model.Price = (int)price;
            model.VAT = _service.GetVAT();
            return model;
        }

        //Termék törlése a kosárból
        public IActionResult RemoveFromCart(int productId)
        {
            List<CartItem> cartItems=GetCartItems();
            if (cartItems != null)
            {
                foreach (CartItem cartItem in cartItems)
                {
                    if (cartItem.ProductId == productId)
                    {
                        cartItems.Remove(cartItem);
                        break;
                    }
                }
                var str = JsonConvert.SerializeObject(cartItems);
                HttpContext.Session.SetString("Cart", str); //A törölt termék nélküli lista mentése
            }
            else
            {
                ClearSessionCartVar();
            }
            return RedirectToAction("Index");
        }

        //Kosár kiűrítése
        public IActionResult ClearCart()
        {
            ClearSessionCartVar();
            return RedirectToAction("Index");
        }
        public void ClearSessionCartVar()
        {
            List<CartItem> cartItems = new List<CartItem>();
            var str = JsonConvert.SerializeObject(cartItems);
            HttpContext.Session.SetString("Cart", str);
        }

    }
}
