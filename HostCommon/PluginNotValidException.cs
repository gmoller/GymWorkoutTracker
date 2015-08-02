using System;

namespace HostCommon
{
    public class PluginNotValidException : Exception
    {
        public PluginNotValidException(Type type, string message)
            : base(string.Format("The plug-in {0} is not valid.\n{1}", type.Name, message))
        {
        }
    }
}