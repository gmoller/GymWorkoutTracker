using DomainModel;

namespace DomainServices
{
    public interface IMuscleRepository : IRepository<Muscle, long>
    {
        Muscle GetByName(string name);
    }
}