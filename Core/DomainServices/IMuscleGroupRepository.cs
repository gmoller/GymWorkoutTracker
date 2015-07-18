using DomainModel;

namespace DomainServices
{
    public interface IMuscleGroupRepository : IRepository<MuscleGroup, long>
    {
        MuscleGroup GetByName(string name);
    }
}