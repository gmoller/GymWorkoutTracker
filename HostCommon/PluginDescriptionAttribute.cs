using System;

namespace HostCommon
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginDescriptionAttribute : Attribute
    {
        private readonly string _description;

        public PluginDescriptionAttribute(string description)
        {
            _description = description;
        }

        public override string ToString()
        {
            return _description;
        }
    }
}