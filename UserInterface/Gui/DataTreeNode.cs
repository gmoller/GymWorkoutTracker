﻿using System;
using System.Windows.Forms;
using HostCommon;

namespace Gui
{
    public class DataTreeNode : TreeNode
    {
        private readonly IPluginData _data;
        public IPluginData Data { get { return _data; } }

        public DataTreeNode(IPluginData data)
        {
            _data = data;
            Text = _data.ToString();
            _data.DataChanged += Data_DataChanged;
        }

        private void Data_DataChanged(object sender, EventArgs e)
        {
            Text = _data.ToString();
        }
    }
}