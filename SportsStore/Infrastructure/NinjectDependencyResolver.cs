﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using SportsStoreDomain.Entities;
using SportsStoreDomain.Abstract;

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
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
                 new Product { Name = "football", Price = 25 },
                 new Product { Name = "newbalance", Price = 179 },
                 new Product { Name = "kingstone", Price =96  }
                });
            this._kernel.Bind<IProductsRepository>().ToConstant(mock.Object);
        }
    }
}