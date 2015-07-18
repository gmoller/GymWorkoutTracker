using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel;
using DomainServices;

namespace DataAccess.InMemory
{
    public class BodyPartInMemoryRepository : IBodyPartRepository
    {
        private readonly Dictionary<long, BodyPart> _bodyParts = new Dictionary<long, BodyPart>();

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            entity.Id = _bodyParts.Count() + 1;
            _bodyParts.Add(entity.Id, (BodyPart)entity);

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _bodyParts[entity.Id] = (BodyPart)entity;

            return entity;
        }

        public void Delete(long id)
        {
            _bodyParts.Remove(id);
        }

        public void Save()
        {
            // do nothing
        }

        public BodyPart Get(long id)
        {
            BodyPart entity;
            try
            {
                entity = _bodyParts[id];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("Data not found.");
            }

            return entity;
        }

        public List<BodyPart> GetAll()
        {
            return _bodyParts.Select(item => item.Value).ToList();
        }

        public BodyPart GetByName(string name)
        {
            return GetAll().FirstOrDefault(item => item.Name == name);
        }
    }
}