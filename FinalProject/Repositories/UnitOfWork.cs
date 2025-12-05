using FinalProject.Entities;
using FinalProject.Interfaces;
using FinalProject.Models;

namespace FinalProject.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly EcommerceContext db;
        private IUserRepository userRepository;
        private IProductRepository productRepository;
        private ICategoryRepository categoryRepository;
        private IGenericRepository<Order> orderRepository;
        private ICartRepository cartRepository;
        private ICartProductRepository cartProductRepository;
        private IGenericRepository<OrderItem> orderItemRepository;

        public UnitOfWork(EcommerceContext _db)
        {
            db = _db;
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(db);
                }
                return userRepository;
            }
        }
        public IProductRepository ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(db);
                }
                return productRepository;
            }
        }
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new CategoryRepository(db);
                }
                return categoryRepository;
            }
        }
        public IGenericRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order>(db);
                }
                return orderRepository;
            }
        }
        public ICartRepository CartRepository
        {
            get
            {
                if (cartRepository == null)
                {
                    cartRepository = new CartRepository(db);
                }
                return cartRepository;
            }
        }
        public ICartProductRepository CartProductRepository
        {
            get
            {
                if (cartProductRepository == null)
                {
                    cartProductRepository = new CartProductRepository(db);
                }
                return cartProductRepository;
            }
        }
        public IGenericRepository<OrderItem> OrderItemRepository
        {
            get
            {
                if (orderItemRepository == null)
                {
                    orderItemRepository = new GenericRepository<OrderItem>(db);
                }
                return orderItemRepository;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}
