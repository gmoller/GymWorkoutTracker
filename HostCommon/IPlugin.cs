namespace HostCommon
{
    public interface IPlugin
    {
        IPluginData[] GetData();
        PluginDataEditControl GetEditControl(IPluginData data);
    }
}