using Catalog.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProducts(string id);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
        Task CreateProduct(Product product);
    }
}
