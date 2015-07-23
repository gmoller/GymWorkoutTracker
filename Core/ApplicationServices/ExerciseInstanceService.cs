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
            exerciseInstance.Validate();

            if (exerciseInstance.Exercise.Id == 0)
            {
                LazyCreateExercise(exerciseInstance);
            }

            return base.Create(exerciseInstance);
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