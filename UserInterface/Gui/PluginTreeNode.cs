using System;
using System.Windows.Forms;
using HostCommon;

namespace Gui
{
    public class PluginTreeNode : TreeNode
    {
        private readonly Type _type;
        private IPlugin _instance;

        public PluginTreeNode(Type type)
        {
            _type = type;
            _instance = (IPlugin)Activator.CreateInstance(_type);

            IPluginData[] data = _instance.GetData();
            foreach (IPluginData item in data)
            {
                Nodes.Add(new DataTreeNode(item));

                if (item.Children != null)
                {
                    foreach (IPluginData childItem in item.Children)
                    {
                        Nodes.Add(new DataTreeNode(childItem));
                    }
                }
            }

            Text = DisplayName;
        }

        public string DisplayName
        {
            get { return _type.GetCustomAttributes(typeof(PluginDisplayNameAttribute), false)[0].ToString(); }
        }

        public string Description
        {
            get { return _type.GetCustomAttributes(typeof(PluginDescriptionAttribute), false)[0].ToString(); }
        }

        public IPlugin Instance
        {
            get { return _instance ?? (_instance = (IPlugin)Activator.CreateInstance(_type)); }
        }
    }
}