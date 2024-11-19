using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void RemoveProduct(Product product);
        List<Product> GetProducts();
    }
}
