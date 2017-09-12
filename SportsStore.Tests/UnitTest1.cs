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
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;

            //断言
            Product[] proArray = result.Products.ToArray();
            Assert.IsTrue(proArray.Length == 2);
            Assert.AreEqual(proArray[0].Name, "p3");
            Assert.AreEqual(proArray[1].Name, "p4");
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
            Assert.AreEqual(expectString, result.ToString());
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
            ProductListViewModel result = (ProductListViewModel)controller.List(null, 2).Model;

            //断言
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 2);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 3);
        }

        [TestMethod]
        public void CanFilterProducts()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="p1" ,Category="cat1"},
                new Product {ProductID=2,Name="p2" ,Category="cat2"},
                new Product {ProductID=3,Name="p3" ,Category="cat1"},
                new Product {ProductID=4,Name="p4" ,Category="cat2"},
                new Product {ProductID=5,Name="p5" ,Category="cat3"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 2;

            //动作
            Product[] result = ((ProductListViewModel)controller.List("cat2", 1).Model).Products.ToArray();

            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "p2" && result[0].Category == "cat2");
            Assert.IsTrue(result[1].Name == "p4" && result[1].Category == "cat2");
        }

        [TestMethod]
        public void CanCreateCategories()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="p1" ,Category="Apples"},
                new Product {ProductID=2,Name="p2" ,Category="Apples"},
                new Product {ProductID=3,Name="p3" ,Category="Plums"},
                new Product {ProductID=4,Name="p4" ,Category="Oranges"},
            });
            NavController target = new NavController(mock.Object);

            //动作
            string[] result = ((IEnumerable<string>)target.Menu().Model).ToArray();

            //断言
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0],"Apples");
            Assert.AreEqual(result[1], "Oranges");
            Assert.AreEqual(result[2], "Plums");
        }

        [TestMethod]
        public void IndicatesSelectedCategory()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="p1" ,Category="Apples"},
                new Product {ProductID=4,Name="p2" ,Category="Oranges"}
            });
            NavController target = new NavController(mock.Object);
            string categoryToSelect = "Apples";

            //动作
            string result = target.Menu(categoryToSelect).ViewBag.SelectCategory;

            //断言
            Assert.AreEqual(categoryToSelect, result);
        }
    }
}
