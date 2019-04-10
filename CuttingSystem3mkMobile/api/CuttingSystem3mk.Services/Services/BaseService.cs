using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Services.Services
{
    public class BaseService<TService, TRepository, TEntity>
         where TEntity : IDomain, new()
         where TRepository : class, new()
         where TService : class
    { 
        private readonly BaseRepository<TEntity, TRepository> _repository;

        public BaseService
            (
                BaseRepository<TEntity, TRepository> baseRepository,
                Repository repository
            )
        {
            Repository = repository;
            _repository = baseRepository;
        }
        #region Methods
        public async Task<TEntity> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> expression = null)
        {
            return await _repository.Get(expression);
        }

        public async Task<IEnumerable<TEntity>> CreateRange(IEnumerable<TEntity> entities)
        {
            return await _repository.CreateRange(entities);
        }
        
        public async Task<TEntity> Create(TEntity entity)
        {
            return await _repository.Create(entity);
        }
        public async Task<TEntity> Update(TEntity entity)
        {
            return await _repository.Update(entity);
        }
        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }
        #endregion

        protected Repository Repository;
    }
}

