using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using HostCommon;

namespace Gui
{
    public partial class MainForm : Form
    {
        private readonly TreeView _treeview;
        private readonly Panel _panel;
        private readonly StatusBar _status;

        public MainForm()
        {
            AddMenu();

            SplitContainer splitContainer = CreateSplitContainer();
            _treeview = CreateTreeView();
            _panel = CreatePanel();
            _status = CreateStatusBar();
            AddTreeViewToSplitContainer(splitContainer, _treeview);
            AddPanelToSplitContainer(splitContainer, _panel);

            Controls.Add(splitContainer);
            Controls.Add(_status);

            LoadPlugins(_treeview);

            Size = new Size(1024, 768);
            Text = @"Host Application";
            _status.Panels[0].Width = _status.Width;

            InitializeComponent();
        }

        private StatusBar CreateStatusBar()
        {
            var status = new StatusBar { ShowPanels = true };
            status.Panels.Add("Description");

            return status;
        }

        private void AddMenu()
        {
            Menu = new MainMenu();
            var file = new MenuItem("File");
            file.MenuItems.AddRange(new[]
                {
                    new MenuItem("-"),
                    new MenuItem("Exit", MenuItemExit_Clicked)
                });
            Menu.MenuItems.Add(file);
        }

        private SplitContainer CreateSplitContainer()
        {
            var splitContainer = new SplitContainer
                {
                    Dock = DockStyle.Fill,
                    ForeColor = SystemColors.Control,
                    Name = "splitContainer",
                    Panel1MinSize = 30,
                    Panel2MinSize = 20,
                    SplitterDistance = 30,
                    TabIndex = 0,
                    Text = @"splitContainer"
                };

            splitContainer.SplitterMoved += splitContainer_SplitterMoved;
            splitContainer.SplitterMoving += splitContainer_SplitterMoving;

            return splitContainer;
        }

        private TreeView CreateTreeView()
        {
            var treeView = new TreeView { Dock = DockStyle.Fill, Name = "treeView", TabIndex = 1 };
            treeView.AfterSelect += Tree_AfterSelect;

            return treeView;
        }

        private Panel CreatePanel()
        {
            var panel = new Panel { Dock = DockStyle.Fill, BorderStyle = BorderStyle.Fixed3D };

            return panel;
        }

        private void AddTreeViewToSplitContainer(SplitContainer splitContainer, TreeView treeView)
        {
            splitContainer.Panel1.Name = "splitterPanel1";
            splitContainer.Panel1.Controls.Add(treeView);
        }

        private void AddPanelToSplitContainer(SplitContainer splitContainer, Panel panel)
        {
            splitContainer.Panel2.Name = "splitterPanel2";
            splitContainer.Panel2.Controls.Add(panel);
        }

        private void LoadPlugins(TreeView treeView)
        {
            string[] files = Directory.GetFiles("Plugins", "*.dll");

            foreach (string f in files)
            {
                try
                {
                    Assembly a = Assembly.LoadFrom(f);
                    Type[] types = a.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.GetInterface("IPlugin") != null)
                        {
                            if (type.GetCustomAttributes(typeof(PluginDisplayNameAttribute), false).Length != 1)
                            {
                                throw new PluginNotValidException(type, "PluginDisplayNameAttribute is not supported.");
                            }
                            if (type.GetCustomAttributes(typeof(PluginDescriptionAttribute), false).Length != 1)
                            {
                                throw new PluginNotValidException(type, "PluginDescriptionAttribute is not supported.");
                            }
                            treeView.Nodes.Add(new PluginTreeNode(type));
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void MenuItemExit_Clicked(object sender, EventArgs e)
        {
            Close();
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (Control c in _panel.Controls)
            {
                c.Dispose();
            }
            _panel.Controls.Clear();

            if (IsNodeTheRootNode(e.Node))
            {
                PluginTreeNode node = GetAsRootNode(e.Node);
                _status.Panels[0].Text = node.Description;
            }
            else
            {
                if (IsNodeABranchNode(e.Node))
                {
                    BranchTreeNode node = GetAsBranchNode(e.Node);
                    _status.Panels[0].Text = node.Text;
                }
                else
                {
                    if (IsNodeALeafNode(e.Node))
                    {
                        LeafTreeNode node = GetAsLeafNode(e.Node);

                        PluginDataEditControl control = ((PluginTreeNode)node.Parent.Parent).Instance.GetEditControl((node).Data);
                        control.Dock = DockStyle.Fill;
                        _panel.Controls.Add(control);
                    }
                }
            }

            _treeview.ExpandAll();
        }

        private bool IsNodeTheRootNode(TreeNode node)
        {
            TreeNode n = GetAsRootNode(node);

            return (n != null);
        }

        private bool IsNodeABranchNode(TreeNode node)
        {
            TreeNode n = GetAsBranchNode(node);

            return (n != null);
        }

        private bool IsNodeALeafNode(TreeNode node)
        {
            TreeNode n = GetAsLeafNode(node);

            return (n != null);
        }

        private PluginTreeNode GetAsRootNode(TreeNode selectedNode)
        {
            var node = selectedNode as PluginTreeNode;

            return node;
        }

        private BranchTreeNode GetAsBranchNode(TreeNode selectedNode)
        {
            var node = selectedNode as BranchTreeNode;

            return node;
        }

        private LeafTreeNode GetAsLeafNode(TreeNode selectedNode)
        {
            var node = selectedNode as LeafTreeNode;

            return node;
        }

        private void splitContainer_SplitterMoving(Object sender, SplitterCancelEventArgs e)
        {
            Cursor.Current = Cursors.NoMoveVert;
        }

        private void splitContainer_SplitterMoved(Object sender, SplitterEventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }
    }
}