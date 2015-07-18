using System.Collections.Generic;
using DomainModel;

namespace DomainServices
{
    public interface IReader<TEntity, TKey> where TEntity : class, IDomainIdentifiable<TKey>
    {
        TEntity Get(TKey id);
        List<TEntity> GetAll();
    }
}