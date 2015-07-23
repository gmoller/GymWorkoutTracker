using System.Collections.Generic;

namespace DomainModel
{
    public class MuscleGroup : IDomainIdentifiable<long>
    {
        private readonly List<Muscle> _muscles;

        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Muscle> Muscles
        {
            get { return _muscles; }
        }

        public MuscleGroup(string name)
        {
            Name = name;
            _muscles = new List<Muscle>();
        }
    }
}