using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository _repository;

        public ProductController(IProductsRepository productRepository)
        {
            this._repository = productRepository;
        }

        public ActionResult List()
        {
            return View(this._repository.Products);
        }
    }
}