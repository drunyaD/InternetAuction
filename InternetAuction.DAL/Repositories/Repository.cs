using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.Interfaces;

namespace InternetAuction.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
        {
            DbContext _context;
            DbSet<T> _dbSet;

            public Repository(DbContext context)
            {
                _context = context;
                _dbSet = context.Set<T>();
            }
            public void Create(T item)
            {
                _dbSet.Add(item);
            }

            public void Update(T item)
            {
                _context.Entry(item).State = EntityState.Modified;
        }

            public void Delete(int id)
            {
            //delete (T item)?
                T item = _dbSet.Find(id);
                _dbSet.Remove(item);
            }

            public IEnumerable<T> Find(Func<T, bool> predicate)
            {
                return _dbSet.Where(predicate).ToList();
            }

            public T Get(int id)
            {
                return _dbSet.Find(id);
            }

            public IQueryable<T> GetQuery()
            {
                return _dbSet.AsQueryable();
            }

            public IEnumerable<T> GetAll()
            {
                return _dbSet.AsNoTracking().ToList();
            }


        }
    }
