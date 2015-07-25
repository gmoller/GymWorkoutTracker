using System.Collections.Generic;
using DomainModel;

namespace DomainServices
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity Get(TKey id);
        List<TEntity> GetAll();
        IDomainIdentifiable<TKey> Create(IDomainIdentifiable<TKey> entity);
        IDomainIdentifiable<TKey> Update(IDomainIdentifiable<TKey> entity);
        void Delete(TKey id);
        void Save();
    }
}