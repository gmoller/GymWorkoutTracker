using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public interface IBaseService<TEntity, TKey> where TEntity : class, IDomainIdentifiable<TKey>
    {
        IReader<TEntity, TKey> Reader { get; }
        IDomainIdentifiable<long> Create(IDomainIdentifiable<long> note);
        IDomainIdentifiable<long> Update(IDomainIdentifiable<long> note);
        void Delete(TKey id);
    }
}