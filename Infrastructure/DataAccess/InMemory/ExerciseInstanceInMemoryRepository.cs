using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainServices;

namespace DataAccess.InMemory
{
    public class ExerciseInstanceInMemoryRepository : IExerciseInstanceRepository
    {
        private readonly Dictionary<long, ExerciseInstance> _exerciseInstances = new Dictionary<long, ExerciseInstance>();

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity.Id = _exerciseInstances.Count() + 1;
            _exerciseInstances.Add(entity.Id, (ExerciseInstance)entity);

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _exerciseInstances[entity.Id] = (ExerciseInstance)entity;

            return entity;
        }

        public void Delete(long id)
        {
            _exerciseInstances.Remove(id);
        }

        public void Save()
        {
            // do nothing
        }

        public ExerciseInstance Get(long id)
        {
            ExerciseInstance entity;
            try
            {
                entity = _exerciseInstances[id];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Data not found.");
            }

            return entity;
        }

        public List<ExerciseInstance> GetAll()
        {
            return _exerciseInstances.Select(item => item.Value).ToList();
        }

        public ExerciseInstance GetByDateTime(DateTime dateTime)
        {
            return GetAll().FirstOrDefault(item => item.Date == dateTime);
        }

        public List<ExerciseInstance> GetByDates(DateTime fromDate, DateTime toDate)
        {
            return GetAll().Where(item => (item.Date >= fromDate && item.Date <= toDate)).ToList();
        }
    }
}