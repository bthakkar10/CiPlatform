using CI_Platform.Entities.DataModels;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CiDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(CiDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        //public T GetFirstOrDefault(Expression <T, bool> filter)
        //{
        //    IQueryable<T> query = dbSet;
        //    query = query.Where(Convert(filter));
        //    return query.FirstOrDefault();
        //}

        //private Expression<Func<T, bool>> Convert(Expression<T, bool> filter)
        //{
        //    throw new NotImplementedException();
        //}

        
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        //public IEnumerable<T> GetAll()
        //{
        //    IQueryable<T> query = dbSet;
        //    return query.ToList();
        //}

        //public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
