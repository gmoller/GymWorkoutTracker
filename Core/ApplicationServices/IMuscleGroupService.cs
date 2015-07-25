using DomainModel;

namespace ApplicationServices
{
    public interface IMuscleGroupService : IBaseService<MuscleGroup, long>
    {
        MuscleGroup GetByName(string name);
    }
}