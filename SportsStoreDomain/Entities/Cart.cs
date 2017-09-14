using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStoreDomain.Entities
{
    public class Cart
    {
        private List<CartLine> _lineCollection = new List<CartLine>();

        public void AddItem(Product product,int quantity)
        {
            CartLine line = this._lineCollection.Where(w => w.Product.ProductID == product.ProductID).FirstOrDefault();
            if(line==null)
            {
                this._lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            this._lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        public decimal ComputerTotalValue()
        {
            return this._lineCollection.Sum(s => s.Product.Price * s.Quantity);
        }

        public void Clear()
        {
            this._lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return this._lineCollection; }
        }
    }
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
