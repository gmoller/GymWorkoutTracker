using System;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class ExerciseService : BaseService<Exercise, long>, IExerciseService
    {
        private readonly IRepository<Muscle, long> _muscleRepository;

        public ExerciseService(IRepository<Exercise, long> repository, IRepository<Muscle, long> muscleRepository)
            : base(repository)
        {
            _muscleRepository = muscleRepository;
        }

        public override IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            var exercise = (Exercise)entity;
            Validate(exercise);

            if (exercise.TargetsMuscle.Id == 0)
            {
                LazyCreateMuscle(exercise);
            }

            return base.Create(exercise);
        }

        private void Validate(Exercise exercise)
        {
            if (exercise.TargetsMuscle == null)
            {
                throw new ApplicationException("Must supply Muscle.");
            }

            if (exercise.TargetsMuscle.Id == 0)
            {
                if (string.IsNullOrEmpty(exercise.TargetsMuscle.Name))
                {
                    throw new ApplicationException("Must supply TargetsMuscle.Name.");
                }
            }
            else if (exercise.TargetsMuscle.Id > 0)
            {
                var muscleService = new MuscleService(_muscleRepository, null);
                Muscle muscle = muscleService.Reader.Get(exercise.TargetsMuscle.Id);
                if (!exercise.TargetsMuscle.Name.Equals(muscle.Name))
                {
                    throw new ApplicationException("Invalid Muscle supplied.");
                }
            }
        }

        private void LazyCreateMuscle(Exercise exercise)
        {
            var muscleService = new MuscleService(_muscleRepository, null);
            var muscleServiceReader = (IMuscleRepository)muscleService.Reader;

            Muscle muscle = muscleServiceReader.GetByName(exercise.TargetsMuscle.Name);
            if (muscle == null)
            {
                muscleService.Create(exercise.TargetsMuscle);
            }
            else
            {
                exercise.TargetsMuscle = muscle;
            }
        }
    }
}