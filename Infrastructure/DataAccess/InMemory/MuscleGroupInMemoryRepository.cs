using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainServices;

namespace DataAccess.InMemory
{
    public class MuscleGroupInMemoryRepository : IMuscleGroupRepository
    {
        private readonly Dictionary<long, MuscleGroup> _muscleGroups = new Dictionary<long, MuscleGroup>();

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity.Id = _muscleGroups.Count() + 1;
            _muscleGroups.Add(entity.Id, (MuscleGroup)entity);

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _muscleGroups[entity.Id] = (MuscleGroup)entity;

            return entity;
        }

        public void Delete(long id)
        {
            _muscleGroups.Remove(id);
        }

        public void Save()
        {
            // do nothing
        }

        public MuscleGroup Get(long id)
        {
            MuscleGroup entity;
            try
            {
                entity = _muscleGroups[id];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Data not found.");
            }

            return entity;
        }

        public List<MuscleGroup> GetAll()
        {
            return _muscleGroups.Select(item => item.Value).ToList();
        }

        public MuscleGroup GetByName(string name)
        {
            return GetAll().FirstOrDefault(item => item.Name == name);
        }
    }
}