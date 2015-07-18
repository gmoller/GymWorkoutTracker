using System;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class ExerciseService : BaseService<Exercise, long>, IExerciseService
    {
        private readonly IRepository<Target, long> _targetRepository;

        public ExerciseService(IRepository<Exercise, long> repository, IRepository<Target, long> targetRepository)
            : base(repository)
        {
            _targetRepository = targetRepository;
        }

        public override IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            var exercise = (Exercise)entity;
            Validate(exercise);

            if (exercise.Target.Id == 0)
            {
                LazyCreateTarget(exercise);
            }

            return base.Create(exercise);
        }

        private void Validate(Exercise exercise)
        {
            if (exercise.Target == null)
            {
                throw new ApplicationException("Must supply Target.");
            }

            if (exercise.Target.Id == 0)
            {
                if (string.IsNullOrEmpty(exercise.Target.Name))
                {
                    throw new ApplicationException("Must supply Target name.");
                }
            }
            else if (exercise.Target.Id > 0)
            {
                var targetService = new TargetService(_targetRepository, null);
                Target target = targetService.Reader.Get(exercise.Target.Id);
                if (!exercise.Target.Name.Equals(target.Name))
                {
                    throw new ApplicationException("Invalid Target supplied.");
                }
            }
        }

        private void LazyCreateTarget(Exercise exercise)
        {
            var targetService = new TargetService(_targetRepository, null);
            var targetServiceReader = (ITargetRepository)targetService.Reader;

            Target target = targetServiceReader.GetByName(exercise.Target.Name);
            if (target == null)
            {
                targetService.Create(exercise.Target);
            }
            else
            {
                exercise.Target = target;
            }
        }
    }
}