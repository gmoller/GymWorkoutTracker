using System.Collections.Generic;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public abstract class BaseService<TEntity, TKey> where TEntity : class, IDomainIdentifiable<TKey>
    {
        protected IRepository<TEntity, TKey> Repository { get; set; }

        protected BaseService(IRepository<TEntity, TKey> repository)
        {
            Repository = repository;
        }

        public virtual List<TEntity> GetAll()
        {
            return Repository.GetAll();
        }

        public virtual TEntity GetById(TKey id)
        {
            return Repository.Get(id);
        }

        public virtual IDomainIdentifiable<TKey> Create(IDomainIdentifiable<TKey> entity)
        {
            entity = Repository.Create(entity);
            Repository.Save();

            return entity;
        }

        public virtual IDomainIdentifiable<TKey> Update(IDomainIdentifiable<TKey> entity)
        {
            Repository.Update(entity);
            Repository.Save();

            return entity;
        }

        public virtual void Delete(TKey id)
        {
            Repository.Delete(id);
            Repository.Save();
        }
    }
}