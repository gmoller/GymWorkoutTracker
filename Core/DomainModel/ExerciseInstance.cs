using System;

namespace DomainModel
{
    public class ExerciseInstance : IDomainIdentifiable<long>
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public Exercise Exercise { get; set; }
        public int Set { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }
    }
}