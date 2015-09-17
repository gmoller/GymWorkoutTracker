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
            chart.Legends.Add(new Legend("Legend"));

            var area = new ChartArea("Main")
            {
                BackColor = Color.White,
                BackSecondaryColor = Color.LightSteelBlue,
                BackGradientStyle = GradientStyle.DiagonalRight,
                AxisY = { Maximum = 100 },
                AxisY2 = { Maximum = 20 }
            };

            area.AxisX.MajorGrid.LineColor = Color.LightSlateGray;
            area.AxisX.TitleFont = new Font("Verdana", 10.0f, FontStyle.Bold);
            area.AxisX.Title = "Date";
            area.AxisY.MajorGrid.LineColor = Color.LightSlateGray;
            area.AxisY.TitleFont = new Font("Verdana", 10.0f, FontStyle.Bold);
            area.AxisY.Title = "Weight";
            area.AxisY2.Title = "Reps";

            chart.ChartAreas.Add(area);

            var seriesColumns1 = new Series("Weights") { ChartType = SeriesChartType.Line, IsValueShownAsLabel = true };
            chart.Series.Add(seriesColumns1);
            var seriesColumns2 = new Series("Reps") { ChartType = SeriesChartType.Line };
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
            double axisYMaximum = 0.0f;
            double axisYMinimum = float.MaxValue;

            foreach (ExerciseInstance exerciseInstance in exerciseInstances)
            {
                chart.Series["Weights"].Points.AddXY(exerciseInstance.Date, exerciseInstance.OneRepMax);
                chart.Series["Reps"].Points.AddXY(exerciseInstance.Date, exerciseInstance.Reps);

                if (exerciseInstance.OneRepMax < axisYMinimum)
                {
                    axisYMinimum = exerciseInstance.OneRepMax - (exerciseInstance.OneRepMax * 0.1f);
                }

                if (exerciseInstance.OneRepMax > axisYMaximum)
                {
                    axisYMaximum = exerciseInstance.OneRepMax * 1.1f;
                }
                exrxName = exerciseInstance.Exercise.ExRxName;
            }

            chart.Titles[0].Text = exrxName;
            chart.ChartAreas[0].AxisY.Minimum = axisYMinimum;
            chart.ChartAreas[0].AxisY.Maximum = axisYMaximum;
        }
    }
}