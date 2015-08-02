using System.Windows.Forms;

namespace HostCommon
{
    public abstract partial class PluginDataEditControl : UserControl
    {
        protected IPluginData Data;

        protected PluginDataEditControl(IPluginData data)
        {
            Data = data;
        }
    }
}