using System;

namespace HostCommon
{
    public interface IPluginData
    {
        event EventHandler DataChanged;

        IPluginData[] Children { get; set; }
    }
}