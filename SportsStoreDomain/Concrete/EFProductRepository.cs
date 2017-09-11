using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStoreDomain.Abstract;
using SportsStoreDomain.Entities;

namespace SportsStoreDomain.Concrete
{
    public class EFProductRepository : IProductsRepository
    {
        private EFDbContext context = new EFDbContext();
        IEnumerable<Product> IProductsRepository.Products
        {
            get
            {
                return context.Products;
            }
        }
    }
}
