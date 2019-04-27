using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SekiroModManager.Manager;

namespace SekiroModManager
{
    public partial class Form_Main : Form
    {
        ModManager modManager = new ModManager();
        List<ModInformation> modList;
        List<string> onLoadFiles;

        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            PathOperation.GetGamePath(false);
            InitListView_mods();
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            if(System.Threading.Thread.CurrentThread.CurrentCulture.Name != "zh-CN")
            {
                label_import.Text = Resource_Language.Tip;
                button_SelectAll.Text = Resource_Language.SelectAll;
                button_DeselectAll.Text = Resource_Language.DeSelectAll;
                button_Import.Text = Resource_Language.Import;
                button_Install.Text = Resource_Language.Install;
                button_Uninstall.Text = Resource_Language.Uninstall;
            }
        }

        private void InitListView_mods()
        {
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name != "zh-CN")
            {
                listView_mods.Columns.Add("Name", (int)(this.listView_mods.Width * 0.4),
                            HorizontalAlignment.Center);
                listView_mods.Columns.Add("CreatedTime", (int)(this.listView_mods.Width * 0.39),
                    HorizontalAlignment.Center);
                listView_mods.Columns.Add("Size", (int)(this.listView_mods.Width * 0.2),
                    HorizontalAlignment.Center);
            } else
            {
                listView_mods.Columns.Add("名称", (int)(this.listView_mods.Width * 0.4),
                            HorizontalAlignment.Center);
                listView_mods.Columns.Add("创建时间", (int)(this.listView_mods.Width * 0.39),
                    HorizontalAlignment.Center);
                listView_mods.Columns.Add("大小", (int)(this.listView_mods.Width * 0.2),
                    HorizontalAlignment.Center);
            }
                
            RefreshListView_mods();
        }

        private void RefreshListView_mods()
        {
            listView_mods.BeginUpdate();
            listView_mods.Items.Clear();

            modList = modManager.GetAllModsInformation();
            if (modList.Count > 0)
            {
                foreach (ModInformation information in modList)
                {
                    ListViewItem item = new ListViewItem
                    {
                        Name = information.name,
                        Text = information.name
                    };
                    item.SubItems.Add(information.time);
                    item.SubItems.Add(information.size + "M");
                    listView_mods.Items.Add(item);
                }
            }

            listView_mods.EndUpdate();
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            modManager.ImportMod(path);
            RefreshListView_mods();
        }

        private void Menu_ModListItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == contextMenuStrip_ModList.Items[0])
            {
                string name = listView_mods.SelectedItems[0].Text;
                modManager.DeleteMod(name);
                RefreshListView_mods();
                treeView_information.Nodes.Clear();
                UpdateTextBoxAndButtons();
            }
        }

        private void ChangeSelectedMod(object sender, EventArgs e)
        {
            treeView_information.Nodes.Clear();
            ListView.SelectedListViewItemCollection items = listView_mods.SelectedItems;
            if (items.Count > 0)
            {
                string name = items[0].Text;
                ModInformation information = modManager.GetModInformation(name, true);
                List<string> contain = information.contain;
                GetContainNode(contain);
                treeView_information.ExpandAll();
                button_SelectAll_Click(new object(), new EventArgs());
                button_Install.Enabled = true;
                button_Uninstall.Enabled = true;
            }
            else
            {
                button_Install.Enabled = false;
                button_Uninstall.Enabled = false;
            }
            UpdateTextBoxAndButtons();
        }

        private void GetContainNode(List<String> contain)
        {
            foreach (string path in contain)
            {
                //获取路径中真实FullPath
                List<string> fullPaths = new List<string>();
                string[] eles = path.Split('\\');
                string nowPath = string.Empty;
                bool first = true;
                foreach (string ele in eles)
                {
                    if (first)
                    {
                        nowPath += ele;
                        first = false;
                    }
                    else
                        nowPath += "\\" + ele;
                    fullPaths.Add(nowPath);
                }

                //若树状图中没有当前FullPath则创建
                TreeNode preNode = null;
                int count = 0;
                foreach (string fullPath in fullPaths)
                {
                    //若第一次执行
                    if (preNode == null)
                    {
                        //若树状图最底层没有路径
                        if (!treeView_information.Nodes.ContainsKey(fullPath))
                        {
                            //创建
                            preNode = treeView_information.Nodes.Add(fullPath, eles[count]);
                        }
                        else
                        {
                            //查找赋值
                            int i = treeView_information.Nodes.IndexOfKey(fullPath);
                            preNode = treeView_information.Nodes[i];
                        }
                    }
                    else
                    {
                        //从上一位置继续查找、赋值
                        if (!preNode.Nodes.ContainsKey(fullPath))
                        {
                            preNode = preNode.Nodes.Add(fullPath, eles[count]);
                        }
                        else
                        {
                            int i = preNode.Nodes.IndexOfKey(fullPath);
                            preNode = preNode.Nodes[i];
                        }
                    }
                    Console.WriteLine(fullPath);
                    count++;
                }
            }
        }

        private void listView_mods_OnClick(object sender, MouseEventArgs e)
        {
            ListView.SelectedListViewItemCollection items = listView_mods.SelectedItems;
            if (items.Count > 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip_ModList.Show(MousePosition);
                }
            }
        }

        private void treeView_information_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                bool NoFalse = true;
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    if (tn.Checked == false)
                    {
                        NoFalse = false;
                    }
                }
                if (e.Node.Checked == true || NoFalse)
                {
                    foreach (TreeNode tn in e.Node.Nodes)
                    {
                        if (tn.Checked != e.Node.Checked)
                        {
                            tn.Checked = e.Node.Checked;
                        }
                    }
                }
            }
            if (e.Node.Parent != null && e.Node.Parent is TreeNode)
            {
                bool ParentNode = true;
                foreach (TreeNode tn in e.Node.Parent.Nodes)
                {
                    if (tn.Checked == false)
                    {
                        ParentNode = false;
                    }
                }
                if (e.Node.Parent.Checked != ParentNode && (e.Node.Checked == false || e.Node.Checked == true && e.Node.Parent.Checked == false))
                {
                    e.Node.Parent.Checked = ParentNode;
                }
            }
            UpdateTextBoxAndButtons();
        }

        private void button_SelectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in treeView_information.Nodes)
            {
                tn.Checked = true;
            }
        }

        private void button_DeselectAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode tn in treeView_information.Nodes)
            {
                tn.Checked = false;
            }
        }

        private void UpdateTextBoxAndButtons()
        {
            onLoadFiles = new List<string>();
            if (listView_mods.SelectedIndices.Count > 0)
            {
                ModInformation information = modList[listView_mods.SelectedIndices[0]];
                foreach (string path in information.contain)
                {
                    if (IsSelectedInTreeView(path))
                    {
                        onLoadFiles.Add(path);
                    }
                }
                
                if(onLoadFiles.Count > 0)
                {
                    button_Install.Enabled = true;
                    button_Uninstall.Enabled = true;
                } else
                {
                    button_Install.Enabled = false;
                    button_Uninstall.Enabled = false;
                }

                textBox_OnLoad.Text = "欲安装的文件：" + Environment.NewLine;
                textBox_Conflict.Text = "存在冲突的文件：" + Environment.NewLine;
                foreach (string path in onLoadFiles)
                {
                    textBox_OnLoad.Text += path + Environment.NewLine;
                    foreach (ReplacedInformation i in modManager.GetReplacedFiles())
                    { 
                        if (path == i.path)
                        {
                            textBox_Conflict.Text += i.name + " 中的 " + path;
                        }
                    }
                }
            }
            else
            {
                textBox_OnLoad.Text = "";
                textBox_Conflict.Text = "";
            }
        }

        private bool IsSelectedInTreeView(string fullPath)
        {
            TreeNode[] finds = treeView_information.Nodes.Find(fullPath, true);
            if (finds.Length > 0)
            {
                if (finds[0].Checked)
                    return true;
            }
            return false;
        }

        private void button_Install_Click(object sender, EventArgs e)
        {
            modManager.InstallFiles(listView_mods.SelectedItems[0].Text, onLoadFiles);
            UpdateTextBoxAndButtons();
        }

        private void button_Uninstall_Click(object sender, EventArgs e)
        {

            modManager.UnInstallFiles(onLoadFiles,true);
            UpdateTextBoxAndButtons();
        }

        private void button_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择Mod文件";
            fileDialog.Filter = "Zip文件(*.zip)|*.zip";
            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = fileDialog.FileNames;
                foreach(string file in files)
                {
                    modManager.ImportMod(file);
                    RefreshListView_mods();
                    
                }
            }
        }
    }
}
