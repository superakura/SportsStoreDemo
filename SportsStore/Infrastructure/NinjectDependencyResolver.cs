using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using SportsStoreDomain.Entities;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Concrete;
using System.Configuration;
using SportsStore.Infrastructure.Abstract;
using SportsStore.Infrastructure.Concrete;

namespace SportsStore.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            this._kernel = kernelParam;
            AddBindings();
        }

        object IDependencyResolver.GetService(Type serviceType)
        {
            return this._kernel.TryGet(serviceType);
        }

        IEnumerable<object> IDependencyResolver.GetServices(Type serviceType)
        {
            return this._kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            #region 利用moq库进行模拟
            //Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product> {
            //     new Product { Name = "football", Price = 25 },
            //     new Product { Name = "newbalance", Price = 179 },
            //     new Product { Name = "kingstone", Price =96  }
            //    });
            //this._kernel.Bind<IProductsRepository>().ToConstant(mock.Object);
            #endregion

            this._kernel.Bind<IProductsRepository>().To<EFProductRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            this._kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);
            this._kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}