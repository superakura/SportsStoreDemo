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
    public class ProductController : Controller
    {
        private IProductsRepository _repository;
        public int pageSize = 2;

        public ProductController(IProductsRepository productRepository)
        {
            this._repository = productRepository;
        }

        public ViewResult List(int page = 1)
        {
            ProductListViewModel model = new ProductListViewModel();

            model.Products = this._repository.Products
                .OrderBy(o => o.ProductID).Skip((page - 1) * pageSize).Take(pageSize);

            model.PagingInfo = new PagingInfo {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = this._repository.Products.Count()
            };

            return View(model);
        }
    }
}