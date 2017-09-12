using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStoreDomain.Abstract;

namespace SportsStore.Controllers
{
    public class NavController : Controller
    {
        private IProductsRepository _repository;

        public NavController(IProductsRepository repo)
        {
            this._repository = repo;
        }

        public PartialViewResult Menu(string category=null)
        {
            ViewBag.SelectCategory = category;
            IEnumerable<string> categorys = this._repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(o => o);
            return PartialView(categorys);
        }
    }
}