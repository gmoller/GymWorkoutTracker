using System.Collections.Generic;
using DomainModel;

namespace ApplicationServices
{
    public interface IBaseService<TEntity, TKey> where TEntity : class, IDomainIdentifiable<TKey>
    {
        List<TEntity> GetAll();
        TEntity GetById(TKey id);
        IDomainIdentifiable<TKey> Create(IDomainIdentifiable<TKey> entity);
        IDomainIdentifiable<TKey> Update(IDomainIdentifiable<TKey> entity);
        void Delete(TKey id);
    }
}