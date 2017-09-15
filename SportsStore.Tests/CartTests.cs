using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStoreDomain.Entities;
using Moq;
using SportsStoreDomain.Abstract;
using System.Web.Mvc;
using SportsStore.Models;
using SportsStore.Controllers;

namespace SportsStore.Tests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void CanAddNewLines()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            //断言
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void CanAddQuantityForExistingLines()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
            
            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(o=>o.Product.ProductID).ToArray();
            
            //断言
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void CanRemoveLine()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            //动作
            target.RemoveLine(p2);

            //断言
            Assert.AreEqual(target.Lines.Where(w=>w.Product==p2).Count(),0);
            Assert.AreEqual(target.Lines.Count(),2);
        }

        [TestMethod]
        public void CanCalculateCartTotal()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1",Price=100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            //动作
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputerTotalValue();

            //断言
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void CanClearContents()
        {
            //准备
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            //动作
            target.Clear();

            //断言
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void CanAddToCart()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(s => s.Products)
                .Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" } }.AsQueryable());
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object);
            //动作
            target.AddToCart(cart, 1, null);
            //断言
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void AddingProductToCartGoesToCartScreen()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(s => s.Products)
                .Returns(new Product[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" } }.AsQueryable());
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object);
            //动作
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");
            //断言
            Assert.AreEqual(result.RouteValues["action"],"Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void CanViewCartContents()
        {
            //准备
            Cart cart = new Cart();
            CartController target = new CartController(null);
            //动作
            CartIndexViewModel result = target.Index(cart, "myUrl").ViewData.Model as CartIndexViewModel;
            //断言
            Assert.AreEqual(result.Cart,cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}
