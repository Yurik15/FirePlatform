using CuttingSystem3mkMobile.Models;
using CuttingSystem3mkMobile.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Repositories.Repositories
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
        protected readonly Context _context = new Context();
        public async Task<TEntity> GetById(int id)
        {
            return await _context.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> expression = null)
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

        public async Task<TEntity> Create(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
