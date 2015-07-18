using DomainModel;

namespace DomainServices
{
    public interface IRepository<TEntity, TKey> : IReader<TEntity, TKey> where TEntity : class, IDomainIdentifiable<TKey>
    {
        IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity);
        IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity);
        void Delete(TKey id);
        void Save();
    }
}