namespace DomainModel
{
    public class Exercise : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string ExRxName { get; set; }
        public string AlternateName { get; set; }
        public string Url { get; set; }
        public Muscle TargetsMuscle { get; set; }

        public Exercise(string exRxName, string alternateName)
        {
            ExRxName = exRxName;
            AlternateName = alternateName;
        }
    }
}