using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class BodyPartService : BaseService<BodyPart, long>, IBodyPartService
    {
        public BodyPartService(IRepository<BodyPart, long> repository)
            : base(repository)
        {
        }
    }
}