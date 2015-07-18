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
    }
}