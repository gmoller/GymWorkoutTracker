using System;
using DomainModel;

namespace DomainServices
{
    public interface IExerciseInstanceRepository : IRepository<ExerciseInstance, long>
    {
        ExerciseInstance GetByDateTime(DateTime dateTime);
    }
}