using System;
using System.Collections.Generic;
using System.Linq;
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

        public ExerciseInstance GetByDateTime(DateTime dateTime)
        {
            return ((IExerciseInstanceRepository)Repository).GetByDateTime(dateTime);
        }

        public List<ExerciseInstance> GetByDates(DateTime fromDate, DateTime toDate)
        {
            return ((IExerciseInstanceRepository)Repository).GetByDates(fromDate, toDate);
        }

        public List<ExerciseInstance> GetByExerciseId(long exerciseId, int months)
        {
            List<ExerciseInstance> list;
            if (months > 0)
            {
                DateTime date = DateTime.Now;
                DateTime fromDate = date.AddMonths(-months);
                DateTime toDate = date;
                list = ((IExerciseInstanceRepository) Repository).GetByDates(fromDate, toDate);
            }
            else
            {
                list = Repository.GetAll();
            }

            return list.Where(item => item.Exercise.Id == exerciseId).ToList();
        }

        public List<ExerciseInstance> GetByExercise(Exercise exercise, int months)
        {
            return GetByExerciseId(exercise.Id, months);
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
            IExerciseService exerciseService = new ExerciseService(_exerciseRepository, null);
            Exercise exercise = exerciseService.GetByAlternateName(exerciseInstance.Exercise.AlternateName);
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