﻿using System;
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
        public int pageSize = 4;

        public ProductController(IProductsRepository productRepository)
        {
            this._repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductListViewModel model = new ProductListViewModel();

            model.Products = this._repository.Products
                .Where(p=>category==null||p.Category==category)
                .OrderByDescending(o => o.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            model.PagingInfo = new PagingInfo {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems =category==null? this._repository.Products.Count():this._repository.Products
                .Where(w=>w.Category==category)
                .Count()
            };

            model.CurrentCategory = category;

            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = this._repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}