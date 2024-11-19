using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public class Cart
    {
        private readonly IProductRepository _productRepository;

        public Cart(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(Product product)
        {
            _productRepository.AddProduct(product);
        }

        public void RemoveProduct(Product product)
        {
            _productRepository.RemoveProduct(product);
        }

        public decimal SumPrices()
        {
            var products = _productRepository.GetProducts();
            return SumPrices(products);
        }

        private decimal SumPrices(List<Product> products)
        {
            decimal sum = 0;
            foreach (Product product in products)
            {
                sum += product.Price;
            }
            return sum;
        }

        public List<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }
    }
}
