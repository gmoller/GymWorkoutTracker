using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class MuscleGroupService : BaseService<MuscleGroup, long>, IMuscleGroupService
    {
        public MuscleGroupService(IRepository<MuscleGroup, long> repository)
            : base(repository)
        {
        }

        public MuscleGroup GetByName(string name)
        {
            return ((IMuscleGroupRepository)Repository).GetByName(name);
        }
    }
}