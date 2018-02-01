using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;
using SportsStore.Controllers;
using Moq;

namespace SportsStore.Tests
{
    [TestClass]
    public class ImageTests
    {
        //GetImage方法能从存储库中返回正确的MIME类型
        [TestMethod]
        public void CanRetrieveImageData()
        {
            //准备-创建一个带图像数据的Product
            Product prod = new Product
            {
                ProductID=2,
                Name="Test",
                ImageData=new byte[] {},
                ImageMimeType="image/png"
            };

            //准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="p1" },
                prod,
                new Product {ProductID=3,Name="p3" }
            }.AsQueryable());

            //准备-创建控制器
            ProductController target = new ProductController(mock.Object);

            //动作-调用GetImage动作方法
            ActionResult result = target.GetImage(2);

            //断言
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, (result as FileResult).ContentType);
        }

        //请求一个不存在的产品ID时无返回数据
        [TestMethod]
        public void CannotRetrieveImageDataForInvalidID()
        {
            //准备-创建模仿存储库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="p1" },
                new Product {ProductID=2,Name="p2" }
            }.AsQueryable());

            //准备-创建控制器
            ProductController target = new ProductController(mock.Object);

            //动作-调用GetImage动作方法
            ActionResult result = target.GetImage(100);

            //断言
            Assert.IsNull(result);
        }
    }
}
