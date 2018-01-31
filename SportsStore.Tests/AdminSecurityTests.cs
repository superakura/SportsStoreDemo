using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Controllers;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Models;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace SportsStore.Tests
{
    [TestClass]
    public class AdminSecurityTests
    {
        //提供正确用户名、密码时，应该通过认证。
        [TestMethod]
        public void CanLoginWithValidCredentials()
        {
            //准备-创建模仿认证提供器
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "cln")).Returns(true);

            //准备-创建视图模型
            LoginViewModel model = new LoginViewModel
            {
                UserName="admin",
                Password="cln"
            };

            //准备-创建控制器
            AccountController target = new AccountController(mock.Object);

            //动作-使用合法凭据进行认证
            ActionResult result = target.Login(model, "/MyURL");

            //断言
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        //提供错误的用户名、密码，不能通过认证。
        [TestMethod]
        public void CannotLoginWithInvalidCredentials()
        {
            //准备-创建模仿认证提供器
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "xxx")).Returns(false);

            //准备-创建视图模型
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "xxx"
            };

            //准备-创建控制器
            AccountController target = new AccountController(mock.Object);

            //动作-使用合法凭据进行认证
            ActionResult result = target.Login(model, "MyURL");

            //断言
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
