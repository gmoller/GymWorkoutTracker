using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainServices;

namespace DataAccess.InMemory
{
    public class MuscleInMemoryRepository : IMuscleRepository
    {
        private readonly Dictionary<long, Muscle> _muscles = new Dictionary<long, Muscle>();

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity.Id = _muscles.Count() + 1;
            _muscles.Add(entity.Id, (Muscle)entity);

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _muscles[entity.Id] = (Muscle)entity;

            return entity;
        }

        public void Delete(long id)
        {
            _muscles.Remove(id);
        }

        public void Save()
        {
            // do nothing
        }

        public Muscle Get(long id)
        {
            Muscle entity;
            try
            {
                entity = _muscles[id];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Data not found.");
            }

            return entity;
        }

        public List<Muscle> GetAll()
        {
            return _muscles.Select(item => item.Value).ToList();
        }

        public Muscle GetByName(string name)
        {
            return GetAll().FirstOrDefault(item => item.Name == name);
        }
    }
}