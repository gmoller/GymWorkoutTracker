using System;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class TargetService : BaseService<Target, long>, ITargetService
    {
        private readonly IRepository<MuscleGroup, long> _muscleGroupRepository;

        public TargetService(IRepository<Target, long> repository, IRepository<MuscleGroup, long> muscleGroupRepository)
            : base(repository)
        {
            _muscleGroupRepository = muscleGroupRepository;
        }

        public override IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            var target = (Target)entity;
            Validate(target);

            if (target.MuscleGroup.Id == 0)
            {
                LazyCreateMuscleGroup(target);
            }

            return base.Create(target);
        }

        private void Validate(Target target)
        {
            if (target.MuscleGroup == null)
            {
                throw new ApplicationException("Must supply MuscleGroup.");
            }

            if (target.MuscleGroup.Id == 0)
            {
                if (string.IsNullOrEmpty(target.MuscleGroup.Name))
                {
                    throw new ApplicationException("Must supply MuscleGroup name.");
                }
            }
            else if (target.MuscleGroup.Id > 0)
            {
                var muscleGroupService = new MuscleGroupService(_muscleGroupRepository);
                MuscleGroup muscleGroup = muscleGroupService.Reader.Get(target.MuscleGroup.Id);
                if (!target.MuscleGroup.Name.Equals(muscleGroup.Name))
                {
                    throw new ApplicationException("Invalid MuscleGroup supplied.");
                }
            }
        }

        private void LazyCreateMuscleGroup(Target target)
        {
            var muscleGroupService = new MuscleGroupService(_muscleGroupRepository);
            var muscleGroupServiceReader = (IMuscleGroupRepository)muscleGroupService.Reader;

            MuscleGroup muscleGroup = muscleGroupServiceReader.GetByName(target.MuscleGroup.Name);
            if (muscleGroup == null)
            {
                muscleGroupService.Create(target.MuscleGroup);
            }
            else
            {
                target.MuscleGroup = muscleGroup;
            }
        }
    }
}