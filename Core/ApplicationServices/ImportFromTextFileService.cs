using System;
using System.Collections.Generic;
using System.IO;
using DomainModel;
using DomainServices;

namespace ApplicationServices
{
    public class ImportFromTextFileService : IImportFromTextFileService
    {
        private readonly IRepository<ExerciseInstance, long> _exerciseInstanceRepository;
        private readonly IRepository<Exercise, long> _exerciseRepository;

        public ImportFromTextFileService(IRepository<ExerciseInstance, long> exerciseInstanceRepository, IRepository<Exercise, long> exerciseRepository)
        {
            _exerciseInstanceRepository = exerciseInstanceRepository;
            _exerciseRepository = exerciseRepository;
        }

        public void Import(string filename)
        {
            List<Line> textLines;

            using (StreamReader sr = File.OpenText(filename))
            {
                textLines = ReadEachLineIntoAList(sr);
            }

            textLines.Reverse();

            foreach (Line line in textLines)
            {
                CreateExerciseInstance(line.DateTime, line.ExerciseName, line.Set, line.Reps, line.Weight);
            }
        }

        private void CreateExerciseInstance(DateTime date, string exerciseName, int set, int reps, float weight)
        {
            var service = new ExerciseInstanceService(_exerciseInstanceRepository, _exerciseRepository);
            var exerciseInstanceServiceReader = (IExerciseInstanceRepository)service.Reader;
            ExerciseInstance exerciseInstance = exerciseInstanceServiceReader.GetByDateTime(date);
            if (exerciseInstance == null)
            {
                exerciseInstance = new ExerciseInstance(date, new Exercise("", exerciseName), set, reps, weight);
                service.Create(exerciseInstance);
            }
        }

        private List<Line> ReadEachLineIntoAList(StreamReader sr)
        {
            var textLines = new List<Line>();
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                var line = new Line(s);
                if (line.DateTime != DateTime.MinValue)
                {
                    textLines.Add(line);
                }
            }

            return textLines;
        }

        private class Line
        {
            internal DateTime DateTime { get; private set; }
            internal string ExerciseName { get; private set; }
            internal int Set { get; private set; }
            internal int Reps { get; private set; }
            internal float Weight { get; private set; }

            internal Line(string s)
            {
                string[] pieces = s.Split(';');
                DateTime dateTime;
                if (DateTime.TryParse(string.Format("{0} {1}", pieces[0], pieces[1]), out dateTime))
                {
                    DateTime = dateTime;
                    ExerciseName = pieces[2];
                    Set = Convert.ToInt32(pieces[4]);
                    Reps = Convert.ToInt32(pieces[6]);
                    Weight = Convert.ToSingle(pieces[5]);
                }
                else
                {
                    DateTime = DateTime.MinValue;
                }
            }
        }
    }
}