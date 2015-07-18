using DomainModel;

namespace DomainServices
{
    public interface IBodyPartRepository : IRepository<BodyPart, long>
    {
        BodyPart GetByName(string name);
    }
}