using System;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class MuscleService : BaseService<Muscle, long>, IMuscleService
    {
        private readonly IRepository<MuscleGroup, long> _muscleGroupRepository;

        public MuscleService(IRepository<Muscle, long> repository, IRepository<MuscleGroup, long> muscleGroupRepository)
            : base(repository)
        {
            _muscleGroupRepository = muscleGroupRepository;
        }

        public override IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            var muscle = (Muscle)entity;
            Validate(muscle);

            if (muscle.BelongsToMuscleGroup.Id == 0)
            {
                LazyCreateMuscleGroup(muscle);
            }

            return base.Create(muscle);
        }

        private void Validate(Muscle muscle)
        {
            if (muscle.BelongsToMuscleGroup == null)
            {
                throw new ApplicationException("Must supply MuscleGroup.");
            }

            if (muscle.BelongsToMuscleGroup.Id == 0)
            {
                if (string.IsNullOrEmpty(muscle.BelongsToMuscleGroup.Name))
                {
                    throw new ApplicationException("Must supply MuscleGroup name.");
                }
            }
            else if (muscle.BelongsToMuscleGroup.Id > 0)
            {
                var muscleGroupService = new MuscleGroupService(_muscleGroupRepository);
                MuscleGroup muscleGroup = muscleGroupService.Reader.Get(muscle.BelongsToMuscleGroup.Id);
                if (!muscle.BelongsToMuscleGroup.Name.Equals(muscleGroup.Name))
                {
                    throw new ApplicationException("Invalid MuscleGroup supplied.");
                }
            }
        }

        private void LazyCreateMuscleGroup(Muscle muscle)
        {
            var muscleGroupService = new MuscleGroupService(_muscleGroupRepository);
            var muscleGroupServiceReader = (IMuscleGroupRepository)muscleGroupService.Reader;

            MuscleGroup muscleGroup = muscleGroupServiceReader.GetByName(muscle.BelongsToMuscleGroup.Name);
            if (muscleGroup == null)
            {
                muscleGroupService.Create(muscle.BelongsToMuscleGroup);
            }
            else
            {
                muscle.BelongsToMuscleGroup = muscleGroup;
            }
        }
    }
}