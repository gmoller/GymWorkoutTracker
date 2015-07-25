using System;
using System.Collections.Generic;
using DomainModel;

namespace DomainServices
{
    public interface IExerciseInstanceRepository : IRepository<ExerciseInstance, long>
    {
        ExerciseInstance GetByDateTime(DateTime dateTime);
        List<ExerciseInstance> GetByDates(DateTime fromDate, DateTime toDate);
    }
}