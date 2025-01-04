using System.Linq.Expressions;

namespace ToDo.API.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        //Get all Tasks
        Task<IEnumerable<T>> GetAllAsync(string id);
        //get by id
        Task<T>GetByIdAsync(int id);
        //Add Task
        Task AddAsync(T item);
        //Update
        void Update(T item);
        //Delete
        bool Delete(T item);

    }
}
