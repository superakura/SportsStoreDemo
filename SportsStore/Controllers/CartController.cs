using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductsRepository _repository;
        public CartController (IProductsRepository repo)
        {
            this._repository = repo;
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart==null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public RedirectToRouteResult AddToCart(int productId,string returnUrl)
        {
            Product product = this._repository.Products.FirstOrDefault(f => f.ProductID == productId);
            if (product!=null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId,string returnUrl)
        {
            Product product = this._repository.Products.FirstOrDefault(f => f.ProductID == productId);
            if (product != null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            CartIndexViewModel cartIndexViewModel = new CartIndexViewModel();
            cartIndexViewModel.Cart = GetCart();
            cartIndexViewModel.ReturnUrl = returnUrl;
            return View(cartIndexViewModel);
        }
    }
}