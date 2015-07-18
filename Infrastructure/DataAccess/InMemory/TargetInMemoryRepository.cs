using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainServices;

namespace DataAccess.InMemory
{
    public class TargetInMemoryRepository : ITargetRepository
    {
        private readonly Dictionary<long, Target> _targets = new Dictionary<long, Target>();

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity.Id = _targets.Count() + 1;
            _targets.Add(entity.Id, (Target)entity);

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _targets[entity.Id] = (Target)entity;

            return entity;
        }

        public void Delete(long id)
        {
            _targets.Remove(id);
        }

        public void Save()
        {
            // do nothing
        }

        public Target Get(long id)
        {
            Target entity;
            try
            {
                entity = _targets[id];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Data not found.");
            }

            return entity;
        }

        public List<Target> GetAll()
        {
            return _targets.Select(item => item.Value).ToList();
        }

        public Target GetByName(string name)
        {
            return GetAll().FirstOrDefault(item => item.Name == name);
        }
    }
}