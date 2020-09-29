using FirePlatform.Models;
using FirePlatform.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirePlatform.Repositories.Repositories
{
    public class BaseRepository<TEntity, TRepository>
        where TEntity : IDomain, new()
        where TRepository : class, new()
    {
        public BaseRepository() { }
        private static readonly object padlock = new object();
        private static TRepository instance = null;
        public static TRepository Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TRepository();
                    }
                    return instance;
                }
            }
        }

        #region Methods
        private readonly Context _context = new Context();
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefault(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression != null)
            {
                return await _context.Set<TEntity>()
                        .AsNoTracking()
                        .Where(expression)
                        .ToListAsync();
            }
            return await _context.Set<TEntity>()
                        .AsNoTracking()
                        .ToListAsync();
        }
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression != null)
            {
                return _context.Set<TEntity>()
                        .AsNoTracking()
                        .Where(expression)
                        .ToList();
            }
            return _context.Set<TEntity>()
                        .AsNoTracking()
                        .ToList();
        }

        public IQueryable<TEntity> GetIQueryable(Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression != null)
            {
                return _context.Set<TEntity>()
                        .AsNoTracking()
                        .Where(expression);
            }
            return _context.Set<TEntity>()
                        .AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> CreateRange(IEnumerable<TEntity> entities)
        {

            await _context.Set<TEntity>()
                        .AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return entities;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public TEntity Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public void Delete(int id)
        {
            var entity = GetById(id);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }
        public async Task DeleteAllAsync()
        {
            var items = await GetAsync();
            _context.Set<TEntity>().RemoveRange(items);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
