using ShopProjectMVC.Core.Models;
using ShopProjectMVC.Core.Interfaces;

namespace ShopProjectMVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository _repository;

        public ProductService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> AddProduct(Product product)
        {
            return await _repository.Add(product);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            return await _repository.Update(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _repository.Delete<Product>(id);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _repository.GetById<Product>(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll<Product>().AsEnumerable();
        }

        public async Task<Order> BuyProduct(int userId, int productId)
        {
            var product = await _repository.GetById<Product>(productId);
            if (product == null)
            {
                throw new Exception("Product does not exist.");
            }
            if (product.Count == 0)
            {
                throw new Exception("Product out of stock.");
            }

            product.Count -= 1;
            await _repository.Update(product);

            var user = await _repository.GetById<User>(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var order = new Order
            {
                User = user,
                Product = product,
                CreatedAt = DateTime.Now
            };

            return await _repository.Add(order);
        }

    }
}
