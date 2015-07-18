using System;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class TargetService : BaseService<Target, long>, ITargetService
    {
        private readonly IRepository<BodyPart, long> _bodyPartRepository;

        public TargetService(IRepository<Target, long> repository, IRepository<BodyPart, long> bodyPartRepository)
            : base(repository)
        {
            _bodyPartRepository = bodyPartRepository;
        }

        public override IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            var target = (Target)entity;
            Validate(target);

            if (target.BodyPart.Id == 0)
            {
                LazyCreateBodyPart(target);
            }

            return base.Create(target);
        }

        private void Validate(Target target)
        {
            if (target.BodyPart == null)
            {
                throw new ApplicationException("Must supply BodyPart.");
            }

            if (target.BodyPart.Id == 0)
            {
                if (string.IsNullOrEmpty(target.BodyPart.Name))
                {
                    throw new ApplicationException("Must supply BodyPart name.");
                }
            }
            else if (target.BodyPart.Id > 0)
            {
                var bodyPartService = new BodyPartService(_bodyPartRepository);
                BodyPart bodyPart = bodyPartService.Reader.Get(target.BodyPart.Id);
                if (!target.BodyPart.Name.Equals(bodyPart.Name))
                {
                    throw new ApplicationException("Invalid BodyPart supplied.");
                }
            }
        }

        private void LazyCreateBodyPart(Target target)
        {
            var bodyPartService = new BodyPartService(_bodyPartRepository);
            var bodyPartServiceReader = (IBodyPartRepository)bodyPartService.Reader;

            BodyPart bodyPart = bodyPartServiceReader.GetByName(target.BodyPart.Name);
            if (bodyPart == null)
            {
                bodyPartService.Create(target.BodyPart);
            }
            else
            {
                target.BodyPart = bodyPart;
            }
        }
    }
}