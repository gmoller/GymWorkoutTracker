using System;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class ExerciseInstanceService : BaseService<ExerciseInstance, long>, IExerciseInstanceService
    {
        private readonly IRepository<Exercise, long> _exerciseRepository;

        public ExerciseInstanceService(IRepository<ExerciseInstance, long> repository, IRepository<Exercise, long> exerciseRepository)
            : base(repository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public override IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            var exerciseInstance = (ExerciseInstance)entity;
            Validate(exerciseInstance);

            if (exerciseInstance.Exercise.Id == 0)
            {
                LazyCreateExercise(exerciseInstance);
            }

            return base.Create(exerciseInstance);
        }

        private void Validate(ExerciseInstance exerciseInstance)
        {
            if (exerciseInstance.Exercise == null)
            {
                throw new ApplicationException("Must supply Exercise.");
            }

            if (exerciseInstance.Exercise.Id == 0)
            {
                if (string.IsNullOrEmpty(exerciseInstance.Exercise.AlternateName))
                {
                    throw new ApplicationException("Must supply Exercise alternate name.");
                }
            }
            else if (exerciseInstance.Exercise.Id > 0)
            {
                var exerciseService = new ExerciseService(_exerciseRepository, null);
                Exercise exercise = exerciseService.Reader.Get(exerciseInstance.Exercise.Id);
                if (!exerciseInstance.Exercise.AlternateName.Equals(exercise.AlternateName))
                {
                    throw new ApplicationException("Invalid Exercise supplied.");
                }
            }
        }

        private void LazyCreateExercise(ExerciseInstance exerciseInstance)
        {
            var exerciseService = new ExerciseService(_exerciseRepository, null);
            var exerciseServiceReader = (IExerciseRepository)exerciseService.Reader;

            Exercise exercise = exerciseServiceReader.GetByAlternateName(exerciseInstance.Exercise.AlternateName);
            if (exercise == null)
            {
                exerciseService.Create(exerciseInstance.Exercise);
            }
            else
            {
                exerciseInstance.Exercise = exercise;
            }
        }
    }
}