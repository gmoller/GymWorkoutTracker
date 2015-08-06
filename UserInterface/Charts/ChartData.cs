using System;
using System.Collections.Generic;
using System.Globalization;
using DomainModel;
using HostCommon;

namespace Charts
{
    public class ChartData : IPluginData
    {
        public event EventHandler DataChanged;

        internal ChartData(IDomainIdentifiable<long> parent, List<ChartData> children)
        {
            Parent = parent;
            Children = children == null ? null : children.ToArray();
        }

        public IDomainIdentifiable<long> Parent { get; private set; }
        public IPluginData[] Children { get; set; }

        public override string ToString()
        {
            return Parent.Name;
        }
    }
}