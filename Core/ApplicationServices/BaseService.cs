using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public abstract class BaseService<TEntity, TKey> where TEntity : class, IDomainIdentifiable<TKey>
    {
        private IRepository<TEntity, TKey> Repository { get; set; }

        public IReader<TEntity, TKey> Reader
        {
            get { return Repository; }
        }

        protected BaseService(IRepository<TEntity, TKey> repository)
        {
            Repository = repository;
        }

        public virtual IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity = Repository.Create(entity);
            Repository.Save();

            return entity;
        }

        public virtual IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
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