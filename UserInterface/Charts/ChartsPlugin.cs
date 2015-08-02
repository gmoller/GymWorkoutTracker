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
            IEnumerable<Exercise> allExercises = GetExercises();

            return allExercises.Select(exercise => new ChartData(exercise)).ToArray();
        }

        public PluginDataEditControl GetEditControl(IPluginData data)
        {
            return new ChartControl((ChartData)data);
        }

        private IEnumerable<Exercise> GetExercises()
        {
            var exerciseService = new ExerciseService(new ExerciseRepository(), new MuscleRepository());
            List<Exercise> allExercises = exerciseService.GetAll();

            return allExercises;
        }
    }
}