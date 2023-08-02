using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project.DAL;

namespace SpeedyStayBLL.Repository
{
    public class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        internal Microsoft.EntityFrameworkCore.DbSet<T> DbSet;
        private DbSet<T> Table => _context.Set<T>();
        private bool disposed = false;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity); 
        }

        public async Task<T> AddAsync(T entity)
        {
            DbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
           if(_context.Entry(entity).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
           {
                DbSet.Attach(entity);
           }
           DbSet.Remove(entity);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == Microsoft.EntityFrameworkCore.EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            return entity;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed) 
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
           
        }

        public async Task<T> UpdateAsync(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return (entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Table.ToListAsync();
        }
    }
}
