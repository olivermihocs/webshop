using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrinkStore.Persistence;
using DrinkStore.Persistence.Services;
using DrinkStore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

namespace DrinkStore.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly DrinkStoreService _service;

        public ProductController(DrinkStoreService service)
        {
            _service = service;
        }


        //Termék lekérése
        [HttpGet]
        public IActionResult Index(int Id)
        {
            Product product;
            try
            {
                product = _service.GetProductById(Id);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            ProductViewModel model = new ProductViewModel
            {
                Product = product,
                Quantity = 1
            };
            model.VAT = _service.GetVAT();
            model.PackageList = new List<SelectListItem>();

            List<String> pNames = _service.GetPackagingList(product.Packaging);

            foreach (String pName in pNames)
            {
                model.PackageList.Add(new SelectListItem { Value = pName, Text = pName });
            }

            return View("Index",model);
        }

        //Termék kosárba tétele
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int Id,ProductViewModel viewModel)
        {
            Product product;
            try
            {
                product = _service.GetProductById(Id);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            viewModel.Product = product;
            viewModel.VAT = _service.GetVAT();
            viewModel.PackageList = new List<SelectListItem>();

            List<String> pNames = _service.GetPackagingList(product.Packaging);

            foreach(String pName in pNames)
            {
                viewModel.PackageList.Add(new SelectListItem { Value = pName, Text = pName });
            }



            if (viewModel.Quantity < 1)
            {
                ModelState.AddModelError(string.Empty, "Helytelen művelet.");
                return View("Index", viewModel);
            }


            List<CartItem> cartItems = new List<CartItem>();
            var str = HttpContext.Session.GetString("Cart");
            if(str!=null)
            {
                try
                {
                    cartItems = JsonConvert.DeserializeObject<List<CartItem>>(str);
                }
                catch
                {
                    str = JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("Cart", str);
                    return RedirectToAction("Index", "Home");
                }
                
            }

            bool inCart = false;
            CartError cartError = CartError.None;
            Packaging packaging = _service.GetPackagingByName(viewModel.SelectedPackaging);

            //Megkeressük hogy a termék bent-van e a kosárban
            foreach (CartItem cartItem in cartItems)
            {
                if (cartItem.ProductId == Id) //Ha igen:
                {
                    inCart = true;
                    if (cartItem.Packaging != packaging) //Ha nem egyezik a csomagolás akkor hiba
                    {
                        cartError = CartError.PackagingInvalid;
                        break;
                    }
                    else
                    {
                        if(_service.ValidateStock(Id, viewModel.Quantity,packaging,cartItem.Quantity)) //Ha egyezik a csomagolás és van elég db.
                        {
                            cartItem.Quantity += viewModel.Quantity;
                            str = JsonConvert.SerializeObject(cartItems);
                            HttpContext.Session.SetString("Cart", str);
                            break;
                        }
                        else //Ha nincs elég készleten akkor hiba.
                        {
                            cartError = CartError.QuantityInvalid;
                        }
                    }
                }
            }

            if (!inCart) { //Ha nincs bent a kosárban
                if(_service.ValidateStock(Id, viewModel.Quantity, packaging)){ //Van-e elég készleten
                    cartItems.Add(new CartItem(Id, viewModel.Quantity, packaging));
                    str = JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("Cart", str);
                }
                else //Ha nincs elég készleten, hiba
                {
                    cartError = CartError.QuantityInvalid;
                }
            }

            switch (cartError)
            {
                case CartError.PackagingInvalid:
                    ModelState.AddModelError(string.Empty,"A kosárban már található más kiszerelésben ez a termék.");
                    break;
                case CartError.ProductInvalid:
                    ModelState.AddModelError(string.Empty, "A termék nem elérhető.");
                    break;
                case CartError.QuantityInvalid:
                    ModelState.AddModelError(string.Empty, "Nincs készleten ennyi db.");
                    break;
            }

            if(cartError == CartError.None) //Ha nincs hiba, egy megerősítő üzenetettel jelezzük
            {
                ViewBag.ConfirmationMessage = viewModel.Quantity.ToString() + " " + viewModel.SelectedPackaging + " " + product.Manufacturer + " " + product.Description +" a kosárba került.";
            }

            return View("Index", viewModel);

        }

    }
}
