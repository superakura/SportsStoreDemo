using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;

namespace SportsStore.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductsRepository _repository;

        public AdminController(IProductsRepository repo)
        {
            this._repository = repo;
        }

        public ViewResult Index()
        {
            return View(this._repository.Products.OrderBy(o=>o.Category));
        }

        public ViewResult Edit(int productId)
        {
            Product product = this._repository.Products.FirstOrDefault(f => f.ProductID == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                this._repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} 已经被保存!", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = this._repository.DeleteProduct(productId);
            if (deletedProduct!=null)
            {
                TempData["message"] = string.Format("{0}已经被删除", deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}