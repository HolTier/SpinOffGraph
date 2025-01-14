using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataAccess
{
    internal interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByNameAsync(string name);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
