using System;
using System.Collections.Generic;
using DomainModel;

namespace ApplicationServices
{
    public interface IExerciseInstanceService : IBaseService<ExerciseInstance, long>
    {
        ExerciseInstance GetByDateTime(DateTime dateTime);
        List<ExerciseInstance> GetByDates(DateTime fromDate, DateTime toDate);
        List<ExerciseInstance> GetByExerciseId(long exerciseId, int months);
        List<ExerciseInstance> GetByExercise(Exercise  exercise, int months);
    }
}