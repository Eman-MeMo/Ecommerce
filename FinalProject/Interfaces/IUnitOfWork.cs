using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        ICartRepository CartRepository { get; }
        ICartProductRepository CartProductRepository { get; }
        IGenericRepository<OrderItem> OrderItemRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
