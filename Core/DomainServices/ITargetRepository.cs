using DomainModel;

namespace DomainServices
{
    public interface ITargetRepository : IRepository<Target, long>
    {
        Target GetByName(string name);
    }
}