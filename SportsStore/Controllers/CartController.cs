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
        private IOrderProcessor _orderProcessor;

        public CartController (IProductsRepository repo,IOrderProcessor proc)
        {
            this._repository = repo;
            this._orderProcessor = proc;
        }

        #region 使用模型绑定前，通过GetCart获取cart对象
        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        #endregion

        public RedirectToRouteResult AddToCart(Cart cart,int productId,string returnUrl)
        {
            Product product = this._repository.Products.FirstOrDefault(f => f.ProductID == productId);
            if (product!=null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId,string returnUrl)
        {
            Product product = this._repository.Products.FirstOrDefault(f => f.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            CartIndexViewModel cartIndexViewModel = new CartIndexViewModel();
            cartIndexViewModel.Cart = cart;
            cartIndexViewModel.ReturnUrl = returnUrl;
            return View(cartIndexViewModel);
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult CheckOut()
        {
            return View(new ShippingDetail());
        }

        [HttpPost]
        public ViewResult CheckOut(Cart cart, ShippingDetail shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "对不起，您的购物车为空！");
            }
            if (ModelState.IsValid)
            {
                this._orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}