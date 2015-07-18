namespace DomainModel
{
    public class Exercise : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string ExRxName { get; set; }
        public string AlternateName { get; set; }
        public string Url { get; set; }
        public Target Target { get; set; }
    }
}