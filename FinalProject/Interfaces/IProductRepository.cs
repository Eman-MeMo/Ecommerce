using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCartIdAsync(int cartId);
        Task<Product> GetProductByName(string name);
    }
}
