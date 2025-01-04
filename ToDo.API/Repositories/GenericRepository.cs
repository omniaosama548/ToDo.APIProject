
using Microsoft.EntityFrameworkCore;
using ToDo.API.Context;
using ToDo.API.Models;

namespace ToDo.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
            await _dbContext.AddAsync(item);
            _dbContext.SaveChanges();
        }

        public bool Delete(T item)
        {
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string id)
        {
            if (typeof(T) == typeof(DoTask))
            {
                return (IEnumerable<T>)await _dbContext.Tasks
                    .Where(t => t.UserId == id) // تصفية المهام بناءً على UserId
                    .Include(t => t.User) // تضمين بيانات المستخدم إذا لزم الأمر
                    .ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _dbContext.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
