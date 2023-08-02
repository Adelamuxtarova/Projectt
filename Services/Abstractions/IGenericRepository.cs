using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyStayBLL.Repository
{
    public interface IGenericRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> GetAll();
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        void Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task<T> UpdateAsync(T entity);
        void Delete(T entity);
        Task<T> DeleteAsync(T entity);
    }
}
