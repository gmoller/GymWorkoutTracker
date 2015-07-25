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

        public Muscle GetByName(string name)
        {
            return ((IMuscleRepository)Repository).GetByName(name);
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
                    throw new ApplicationException("Must supply BelongsToMuscleGroup.Name.");
                }
            }
            else if (muscle.BelongsToMuscleGroup.Id > 0)
            {
                var muscleGroupService = new MuscleGroupService(_muscleGroupRepository);
                MuscleGroup muscleGroup = muscleGroupService.GetById(muscle.BelongsToMuscleGroup.Id);
                if (!muscle.BelongsToMuscleGroup.Name.Equals(muscleGroup.Name))
                {
                    throw new ApplicationException("Invalid MuscleGroup supplied.");
                }
            }
        }

        private void LazyCreateMuscleGroup(Muscle muscle)
        {
            IMuscleGroupService muscleGroupService = new MuscleGroupService(_muscleGroupRepository);
            MuscleGroup muscleGroup = muscleGroupService.GetByName(muscle.BelongsToMuscleGroup.Name);
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