
using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using SearchAPI.Data;
using SearchAPI.Models;
using System.Reflection;
using System;

namespace SearchAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private ApplicationDbContext _context = null;
        private DbSet<T> entities = null;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
            entities = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public T GetById(int id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        //public async Task<IEnumerable<T>> Search(string name)
        //{
        //    IQueryable<T> query =  entities.AsQueryable<T>();

        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        query = query.Where(e => e.FirstName.Contains(name)
        //                    || e.LastName.Contains(name));
        //    }

        //    if (gender != null)
        //    {
        //        query = query.Where(e => e.Gender == gender);
        //    }

        //    return await query.ToListAsync();
        //}
    }
}
