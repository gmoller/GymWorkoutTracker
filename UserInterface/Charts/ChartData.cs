using System;
using DomainModel;
using HostCommon;

namespace Charts
{
    public class ChartData : IPluginData
    {
        public event EventHandler DataChanged;

        internal ChartData(Exercise exercise)
        {
            Exercise = exercise;
        }

        public MuscleGroup MuscleGroup { get; private set; }
        public Exercise Exercise { get; private set; }

        public override string ToString()
        {
            return Exercise.ExRxName;
        }


        public IPluginData[] Children
        {
            get { return null; }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}