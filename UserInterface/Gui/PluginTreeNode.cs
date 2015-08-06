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
                if (item.Children == null)
                {
                    Nodes.Add(new LeafTreeNode(item));
                }
                else
                {
                    int i = Nodes.Add(new BranchTreeNode(item));
                    var newNode = Nodes[i];
                    foreach (IPluginData childItem in item.Children)
                    {
                        if (childItem.Children == null)
                        {
                            newNode.Nodes.Add(new LeafTreeNode(childItem));
                        }
                        else
                        {
                            newNode.Nodes.Add(new BranchTreeNode(childItem));
                        }
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