using System.Collections.Generic;
using System.Linq;
using ApplicationServices;
using DatabaseMySql;
using DomainModel;
using HostCommon;

namespace Charts
{
    [PluginDisplayName("Charts")]
    [PluginDescription("This plugin is for showing visualizations of data on a chart")]
    public class ChartsPlugin : IPlugin
    {
        public IPluginData[] GetData()
        {
            IEnumerable<Muscle> allMuscles = GetMuscles();
            IEnumerable<Exercise> allExercises = GetExercises();

            var muscles = new List<ChartData>();
            foreach (Muscle muscle in allMuscles)
            {
                var exercises = new List<ChartData>();
                foreach (Exercise exercise in allExercises)
                {
                    if (muscle.Id == exercise.TargetsMuscle.Id)
                    {
                        exercises.Add(new ChartData(exercise, null));
                    }
                }

                muscles.Add(new ChartData(muscle, exercises));
            }

            return muscles.ToArray();
        }

        public PluginDataEditControl GetEditControl(IPluginData data)
        {
            return new ChartControl((ChartData)data);
        }

        private IEnumerable<Muscle> GetMuscles()
        {
            var muscleService = new MuscleService(new MuscleRepository(), new MuscleGroupRepository());
            List<Muscle> allMuscles = muscleService.GetAll();

            return allMuscles;
        }

        private IEnumerable<Exercise> GetExercises()
        {
            var exerciseService = new ExerciseService(new ExerciseRepository(), new MuscleRepository());
            List<Exercise> allExercises = exerciseService.GetAll();

            return allExercises;
        }
    }
}