using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly EcommerceContext db;
        public GenericRepository(EcommerceContext _db)
        {
            db = _db;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await db.Set<T>().AsNoTracking().ToListAsync();
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            return db.Set<T>().AsQueryable();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            db.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
        }
    }
}
