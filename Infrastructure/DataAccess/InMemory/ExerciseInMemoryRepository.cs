using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainServices;

namespace DataAccess.InMemory
{
    public class ExerciseInMemoryRepository : IExerciseRepository
    {
        private readonly Dictionary<long, Exercise> _exercises = new Dictionary<long, Exercise>();

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity.Id = _exercises.Count() + 1;
            _exercises.Add(entity.Id, (Exercise)entity);

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _exercises[entity.Id] = (Exercise)entity;

            return entity;
        }

        public void Delete(long id)
        {
            _exercises.Remove(id);
        }

        public void Save()
        {
            // do nothing
        }

        public Exercise Get(long id)
        {
            Exercise entity;
            try
            {
                entity = _exercises[id];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Data not found.");
            }

            return entity;
        }

        public List<Exercise> GetAll()
        {
            return _exercises.Select(item => item.Value).ToList();
        }

        public Exercise GetByAlternateName(string name)
        {
            return GetAll().FirstOrDefault(item => item.AlternateName == name);
        }
    }
}