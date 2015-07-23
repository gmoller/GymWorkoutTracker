namespace DomainModel
{
    public class Muscle : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public MuscleGroup BelongsToMuscleGroup { get; set; }

        public Muscle(string name)
        {
            Name = name;
        }
    }
}