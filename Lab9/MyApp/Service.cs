using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyApp
{
    public class Service : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();
        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public List<Product> GetProducts()
        {
            return _products;
        }

        public void RemoveProduct(Product product)
        {
            _products.Remove(product);
        }

    }
}
