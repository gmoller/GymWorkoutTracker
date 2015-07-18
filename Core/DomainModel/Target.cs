namespace DomainModel
{
    public class Target : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public BodyPart BodyPart { get; set; }
    }
}