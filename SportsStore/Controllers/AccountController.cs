using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider _authProvider;

        public AccountController(IAuthProvider auth)
        {
            this._authProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (this._authProvider.Authenticate(model.UserName,model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "用户名，密码不正确！");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}