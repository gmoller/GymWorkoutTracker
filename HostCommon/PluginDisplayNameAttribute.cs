using System;

namespace HostCommon
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginDisplayNameAttribute : Attribute
    {
        private readonly string _displayName;

        public PluginDisplayNameAttribute(string displayName)
        {
            _displayName = displayName;
        }

        public override string ToString()
        {
            return _displayName;
        }
    }
}