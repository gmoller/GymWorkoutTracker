using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ApplicationServices;
using DatabaseMySql;
using DomainModel;
using HostCommon;

namespace Charts
{
    public partial class ChartControl : PluginDataEditControl
    {
        public ChartControl(ChartData chartData)
            : base(chartData)
        {
            Chart chart = AddChartToForm(chartData);
            IEnumerable<ExerciseInstance> exercisesInstances = GetExerciseInstances(chartData.Parent.Id);
            AddDataPointsToChart(exercisesInstances, chart);

            InitializeComponent();
        }

        private Chart AddChartToForm(ChartData chartData)
        {
            var chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var title = new Title(chartData.ToString()) { Font = new Font("Verdana", 14.0f) };
            chart.Titles.Add(title);

            var area = new ChartArea("Main")
            {
                BackColor = Color.White,
                BackSecondaryColor = Color.LightSteelBlue,
                BackGradientStyle = GradientStyle.DiagonalRight,
                AxisY = { Maximum = 100 }
            };

            area.AxisX.MajorGrid.LineColor = Color.LightSlateGray;
            area.AxisY.MajorGrid.LineColor = Color.LightSlateGray;

            chart.ChartAreas.Add(area);

            var seriesColumns1 = new Series("ExerciseInstances.Weights") { ChartType = SeriesChartType.Line };
            chart.Series.Add(seriesColumns1);
            var seriesColumns2 = new Series("ExerciseInstances.Reps") { ChartType = SeriesChartType.Line };
            chart.Series.Add(seriesColumns2);

            Controls.Add(chart);

            return chart;
        }

        private IEnumerable<ExerciseInstance> GetExerciseInstances(long exerciseId)
        {
            var exerciseInstanceService = new ExerciseInstanceService(new ExerciseInstanceRepository(), new ExerciseRepository());
            List<ExerciseInstance> allExerciseInstances = exerciseInstanceService.GetByDates(DateTime.MinValue, DateTime.MaxValue);

            List<ExerciseInstance> exerciseInstances = allExerciseInstances.Where(item => item.Exercise.Id == exerciseId).ToList();

            return exerciseInstances;
        }

        private void AddDataPointsToChart(IEnumerable<ExerciseInstance> exerciseInstances, Chart chart)
        {
            string exrxName = string.Empty;
            float axisYMaximum = 0.0f;
            float axisYMinimum = float.MaxValue;

            foreach (ExerciseInstance exerciseInstance in exerciseInstances)
            {
                chart.Series["ExerciseInstances.Weights"].Points.Add(exerciseInstance.Weight);
                chart.Series["ExerciseInstances.Reps"].Points.Add(exerciseInstance.Reps);

                if (exerciseInstance.Weight < axisYMinimum)
                {
                    axisYMinimum = exerciseInstance.Weight - (exerciseInstance.Weight * 0.05f);
                }

                if (exerciseInstance.Weight > axisYMaximum)
                {
                    axisYMaximum = exerciseInstance.Weight * 1.05f;
                }
                exrxName = exerciseInstance.Exercise.ExRxName;
            }

            chart.Titles[0].Text = exrxName;
            chart.ChartAreas[0].AxisY.Minimum = axisYMinimum;
            chart.ChartAreas[0].AxisY.Maximum = axisYMaximum;
        }
    }
}