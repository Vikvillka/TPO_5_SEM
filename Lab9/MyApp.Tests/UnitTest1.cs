using Moq;
namespace MyApp.Tests
{
    [TestFixture]
    public class Tests
    {
        private Cart cart;
        private Mock<IProductRepository> mockProduct;
        private List<Product> productList;

        [SetUp]
        public void Setup()
        {
            productList = new List<Product>(); 
            mockProduct = new Mock<IProductRepository>();

            mockProduct.Setup(repo => repo.AddProduct(It.IsAny<Product>()))
                      .Callback<Product>(p => productList.Add(p));

            mockProduct.Setup(repo => repo.GetProducts())
                      .Returns(() => productList);

            mockProduct.Setup(repo => repo.RemoveProduct(It.IsAny<Product>()))
                      .Callback<Product>(p => productList.Remove(p));

            cart = new Cart(mockProduct.Object);
        }

        [Test]
        public void AddProduct_ProductActuallyAdded()
        {
            // Arrange
            var testProduct = new Product("Продукт", 10.5m);

            // Act
            cart.AddProduct(testProduct);

            // Assert
            Assert.That(productList.Contains(testProduct), Is.True); 
            Assert.That(productList.Count, Is.EqualTo(1)); 
            Assert.That(cart.GetProducts().First(), Is.EqualTo(testProduct)); 
        }

        [Test]
        public void RemoveProduct_ProductActuallyRemoved()
        {
            // Arrange
            var testProduct = new Product("Продукт", 10.5m);
            cart.AddProduct(testProduct); 
            Assert.That(productList.Count, Is.EqualTo(1)); 

            // Act
            cart.RemoveProduct(testProduct);

            // Assert
            Assert.That(productList.Contains(testProduct), Is.False); 
            Assert.That(productList.Count, Is.EqualTo(0)); 
            Assert.That(cart.GetProducts().Count, Is.EqualTo(0)); 
        }

        [Test]
        public void SumProduct_CalculatesCorrectSum()
        {
            // Arrange
            var testProduct1 = new Product("Продукт1", 10.5m);
            var testProduct2 = new Product("Продукт2", 20.0m);

            // Act
            cart.AddProduct(testProduct1);
            cart.AddProduct(testProduct2);

            // Assert
            Assert.That(productList.Count, Is.EqualTo(2)); 
            Assert.That(cart.SumPrices(), Is.EqualTo(30.5m)); 
            Assert.That(cart.GetProducts(), Is.EquivalentTo(new[] { testProduct1, testProduct2 })); 
        }

        [Test]
        public void GetProducts_ReturnsCorrectProducts()
        {
            // Arrange
            var testProduct1 = new Product("Продукт1", 10.5m);
            var testProduct2 = new Product("Продукт2", 20.0m);

            // Act
            cart.AddProduct(testProduct1);
            cart.AddProduct(testProduct2);
            var products = cart.GetProducts();

            // Assert
            Assert.That(products.Count, Is.EqualTo(2));
            Assert.That(products, Contains.Item(testProduct1));
            Assert.That(products, Contains.Item(testProduct2));
            Assert.That(products, Is.EquivalentTo(productList));
        }
    }
}