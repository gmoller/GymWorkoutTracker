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
            List<MuscleGroup> allMuscleGroups = GetMuscleGroups();
            List<Muscle> allMuscles = GetMuscles();
            List<Exercise> allExercises = GetExercises();

            var data = new List<ChartData>();
            foreach (MuscleGroup muscleGroup in allMuscleGroups)
            {

            }

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

                data.Add(new ChartData(muscle, exercises));
            }

            return data.ToArray();
        }

        public PluginDataEditControl GetEditControl(IPluginData data)
        {
            return new ChartControl((ChartData)data);
        }

        private List<MuscleGroup> GetMuscleGroups()
        {
            var muscleGroupService = new MuscleGroupService(new MuscleGroupRepository());
            List<MuscleGroup> allMuscleGroups = muscleGroupService.GetAll();

            return allMuscleGroups;
        }

        private List<Muscle> GetMuscles()
        {
            var muscleService = new MuscleService(new MuscleRepository(), new MuscleGroupRepository());
            List<Muscle> allMuscles = muscleService.GetAll();

            return allMuscles;
        }

        private List<Exercise> GetExercises()
        {
            var exerciseService = new ExerciseService(new ExerciseRepository(), new MuscleRepository());
            List<Exercise> allExercises = exerciseService.GetAll();

            return allExercises;
        }
    }
}