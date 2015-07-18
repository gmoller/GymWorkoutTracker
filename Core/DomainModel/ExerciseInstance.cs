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

        //public void Validate()
        //{
        //    if (Exercise == null)
        //    {
        //        throw new ApplicationException("Must supply Exercise.");
        //    }

        //    if (Exercise.Id == 0)
        //    {
        //        if (string.IsNullOrEmpty(Exercise.AlternateName))
        //        {
        //            throw new ApplicationException("Must supply Exercise alternate name.");
        //        }
        //    }
        //    else if (Exercise.Id > 0)
        //    {
        //        var exerciseService = new ExerciseService(_exerciseRepository, null);
        //        Exercise exercise = exerciseService.Reader.Get(Exercise.Id);
        //        if (!Exercise.AlternateName.Equals(exercise.AlternateName))
        //        {
        //            throw new ApplicationException("Invalid Exercise supplied.");
        //        }
        //    }
        //}
    }
}