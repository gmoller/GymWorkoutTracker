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

        public ExerciseInstance(DateTime date, Exercise exercise, int set, int reps, float weight)
        {
            Date = date;
            Exercise = exercise;
            Set = set;
            Reps = reps;
            Weight = weight;

            Validate();
        }

        public string Name
        {
            get { return string.Empty; }
            set { }
        }

        public void Validate()
        {
            if (Date <= DateTime.MinValue)
            {
                throw new ApplicationException(string.Format("Must supply Date greater than {0}.", DateTime.MinValue));
            }
            
            if (Exercise == null)
            {
                throw new ApplicationException("Must supply Exercise.");
            }

            if (Exercise.Id == 0 && string.IsNullOrEmpty(Exercise.AlternateName))
            {
                throw new ApplicationException("Must supply Exercise.AlternateName.");
            }

            if (Set <= 0)
            {
                throw new ApplicationException(string.Format("Must supply Set greater than {0}.", 0));
            }

            if (Reps <= 0)
            {
                throw new ApplicationException(string.Format("Must supply Reps greater than {0}.", 0));
            }

            if (Weight < 0)
            {
                throw new ApplicationException(string.Format("Must supply Weight greater than or equal to {0}.", 0));
            }
        }
    }
}