using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;
using SportsStore.Controllers;
using System.Collections.Generic;
using System.Linq;
using SportsStore.Models;
using SportsStore.HtmlHelpers;

namespace SportsStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="p1" },
                new Product {ProductID=2,Name="p2" },
                new Product {ProductID=3,Name="p3" },
                new Product {ProductID=4,Name="p4" },
                new Product {ProductID=5,Name="p5" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 2;

            //动作
            ProductListViewModel result =(ProductListViewModel)controller.List(2).Model;

            //断言
            Product[] proArray = result.Products.ToArray();
            Assert.IsTrue(proArray.Length == 2);
            Assert.AreEqual(proArray[0].Name,"p3");
            Assert.AreEqual(proArray[1].Name,"p4");
        }

        [TestMethod]
        public void CanGeneratePageLinks()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = 2;
            pagingInfo.TotalItems = 28;
            pagingInfo.ItemsPerPage = 10;

            //准备
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //动作
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //断言
            var expectString = @"<a class=""btn btn-default"" href=""Page1"">1</a>";
            expectString += @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>";
            expectString += @"<a class=""btn btn-default"" href=""Page3"">3</a>";
            Assert.AreEqual(expectString,result.ToString());
        }

        [TestMethod]
        public void CanSeedPaginationViewModel()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="p1" },
                new Product {ProductID=2,Name="p2" },
                new Product {ProductID=3,Name="p3" },
                new Product {ProductID=4,Name="p4" },
                new Product {ProductID=5,Name="p5" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 2;

            //动作
           ProductListViewModel result = (ProductListViewModel)controller.List(2).Model;

            //断言
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 2);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 3);
        }
    }
}
