namespace DomainModel
{
    public class MuscleGroup : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}