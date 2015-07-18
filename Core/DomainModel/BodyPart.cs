namespace DomainModel
{
    public class BodyPart : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}