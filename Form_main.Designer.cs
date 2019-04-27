namespace SekiroModManager
{
    partial class Form_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.contextMenuStrip_ModList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_import = new System.Windows.Forms.Label();
            this.treeView_information = new System.Windows.Forms.TreeView();
            this.listView_mods = new System.Windows.Forms.ListView();
            this.button_SelectAll = new System.Windows.Forms.Button();
            this.button_DeselectAll = new System.Windows.Forms.Button();
            this.textBox_OnLoad = new System.Windows.Forms.TextBox();
            this.button_Install = new System.Windows.Forms.Button();
            this.textBox_Conflict = new System.Windows.Forms.TextBox();
            this.button_Uninstall = new System.Windows.Forms.Button();
            this.button_Import = new System.Windows.Forms.Button();
            this.contextMenuStrip_ModList.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_ModList
            // 
            this.contextMenuStrip_ModList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStrip_ModList.Name = "contextMenuStrip_ModList";
            this.contextMenuStrip_ModList.Size = new System.Drawing.Size(101, 26);
            this.contextMenuStrip_ModList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Menu_ModListItemClicked);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // label_import
            // 
            this.label_import.AutoSize = true;
            this.label_import.Font = new System.Drawing.Font("Roboto", 11F);
            this.label_import.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label_import.Location = new System.Drawing.Point(12, 9);
            this.label_import.Name = "label_import";
            this.label_import.Size = new System.Drawing.Size(252, 21);
            this.label_import.TabIndex = 1;
            this.label_import.Text = "将Mod压缩包拖入窗口以导入文件";
            // 
            // treeView_information
            // 
            this.treeView_information.CheckBoxes = true;
            this.treeView_information.Location = new System.Drawing.Point(330, 33);
            this.treeView_information.Name = "treeView_information";
            this.treeView_information.Size = new System.Drawing.Size(299, 454);
            this.treeView_information.TabIndex = 2;
            this.treeView_information.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_information_AfterCheck);
            // 
            // listView_mods
            // 
            this.listView_mods.FullRowSelect = true;
            this.listView_mods.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_mods.Location = new System.Drawing.Point(12, 33);
            this.listView_mods.MultiSelect = false;
            this.listView_mods.Name = "listView_mods";
            this.listView_mods.Size = new System.Drawing.Size(302, 454);
            this.listView_mods.TabIndex = 3;
            this.listView_mods.UseCompatibleStateImageBehavior = false;
            this.listView_mods.View = System.Windows.Forms.View.Details;
            this.listView_mods.SelectedIndexChanged += new System.EventHandler(this.ChangeSelectedMod);
            this.listView_mods.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView_mods_OnClick);
            // 
            // button_SelectAll
            // 
            this.button_SelectAll.Location = new System.Drawing.Point(339, 501);
            this.button_SelectAll.Name = "button_SelectAll";
            this.button_SelectAll.Size = new System.Drawing.Size(133, 28);
            this.button_SelectAll.TabIndex = 4;
            this.button_SelectAll.Text = "全选";
            this.button_SelectAll.UseVisualStyleBackColor = true;
            this.button_SelectAll.Click += new System.EventHandler(this.button_SelectAll_Click);
            // 
            // button_DeselectAll
            // 
            this.button_DeselectAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_DeselectAll.Location = new System.Drawing.Point(491, 503);
            this.button_DeselectAll.Name = "button_DeselectAll";
            this.button_DeselectAll.Size = new System.Drawing.Size(137, 25);
            this.button_DeselectAll.TabIndex = 5;
            this.button_DeselectAll.Text = "取消全选";
            this.button_DeselectAll.UseVisualStyleBackColor = true;
            this.button_DeselectAll.Click += new System.EventHandler(this.button_DeselectAll_Click);
            // 
            // textBox_OnLoad
            // 
            this.textBox_OnLoad.Location = new System.Drawing.Point(640, 33);
            this.textBox_OnLoad.Multiline = true;
            this.textBox_OnLoad.Name = "textBox_OnLoad";
            this.textBox_OnLoad.ReadOnly = true;
            this.textBox_OnLoad.Size = new System.Drawing.Size(436, 149);
            this.textBox_OnLoad.TabIndex = 6;
            // 
            // button_Install
            // 
            this.button_Install.Enabled = false;
            this.button_Install.Location = new System.Drawing.Point(642, 392);
            this.button_Install.Name = "button_Install";
            this.button_Install.Size = new System.Drawing.Size(436, 30);
            this.button_Install.TabIndex = 7;
            this.button_Install.Text = "安装所选文件";
            this.button_Install.UseVisualStyleBackColor = true;
            this.button_Install.Click += new System.EventHandler(this.button_Install_Click);
            // 
            // textBox_Conflict
            // 
            this.textBox_Conflict.Location = new System.Drawing.Point(640, 206);
            this.textBox_Conflict.Multiline = true;
            this.textBox_Conflict.Name = "textBox_Conflict";
            this.textBox_Conflict.ReadOnly = true;
            this.textBox_Conflict.Size = new System.Drawing.Size(435, 132);
            this.textBox_Conflict.TabIndex = 8;
            // 
            // button_Uninstall
            // 
            this.button_Uninstall.Enabled = false;
            this.button_Uninstall.Location = new System.Drawing.Point(646, 438);
            this.button_Uninstall.Name = "button_Uninstall";
            this.button_Uninstall.Size = new System.Drawing.Size(430, 27);
            this.button_Uninstall.TabIndex = 9;
            this.button_Uninstall.Text = "卸载所选文件";
            this.button_Uninstall.UseVisualStyleBackColor = true;
            this.button_Uninstall.Click += new System.EventHandler(this.button_Uninstall_Click);
            // 
            // button_Import
            // 
            this.button_Import.Location = new System.Drawing.Point(12, 501);
            this.button_Import.Name = "button_Import";
            this.button_Import.Size = new System.Drawing.Size(296, 28);
            this.button_Import.TabIndex = 10;
            this.button_Import.Text = "导入";
            this.button_Import.UseVisualStyleBackColor = true;
            this.button_Import.Click += new System.EventHandler(this.button_Import_Click);
            // 
            // Form_Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 604);
            this.Controls.Add(this.button_Import);
            this.Controls.Add(this.button_Uninstall);
            this.Controls.Add(this.textBox_Conflict);
            this.Controls.Add(this.button_Install);
            this.Controls.Add(this.textBox_OnLoad);
            this.Controls.Add(this.button_DeselectAll);
            this.Controls.Add(this.button_SelectAll);
            this.Controls.Add(this.listView_mods);
            this.Controls.Add(this.treeView_information);
            this.Controls.Add(this.label_import);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SeikiroModManager";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.contextMenuStrip_ModList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ModList;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView_information;
        private System.Windows.Forms.Label label_import;
        private System.Windows.Forms.ListView listView_mods;
        private System.Windows.Forms.Button button_SelectAll;
        private System.Windows.Forms.Button button_DeselectAll;
        private System.Windows.Forms.TextBox textBox_OnLoad;
        private System.Windows.Forms.Button button_Install;
        private System.Windows.Forms.TextBox textBox_Conflict;
        private System.Windows.Forms.Button button_Uninstall;
        private System.Windows.Forms.Button button_Import;
    }
}

