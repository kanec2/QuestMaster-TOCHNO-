using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace QuestMaster
{
    partial class Explorer
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.togger1 = new QuestMaster.Togger();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SortFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SortAscend = new System.Windows.Forms.ToolStripMenuItem();
            this.SortDescend = new System.Windows.Forms.ToolStripMenuItem();
            this.NoSort = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewFile = new System.Windows.Forms.ToolStripMenuItem();
            this.LargeIconFile = new System.Windows.Forms.ToolStripMenuItem();
            this.SmallIconFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ListFile = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteFile = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStrip1.Location = new System.Drawing.Point(834, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(26, 521);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // togger1
            // 
            this.togger1.Dock = System.Windows.Forms.DockStyle.Top;
            this.togger1.Location = new System.Drawing.Point(50, 0);
            this.togger1.Name = "togger1";
            this.togger1.Size = new System.Drawing.Size(784, 35);
            this.togger1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(50, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 486F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 486);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(229, 480);
            this.treeView1.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(238, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(543, 480);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddFile,
            this.SortFile,
            this.ViewFile,
            this.DeleteFile});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 114);
            // 
            // AddFile
            // 
            this.AddFile.Name = "AddFile";
            this.AddFile.Size = new System.Drawing.Size(197, 22);
            this.AddFile.Text = "Добавить новый файл";
            this.AddFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // SortFile
            // 
            this.SortFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SortAscend,
            this.SortDescend,
            this.NoSort});
            this.SortFile.Name = "SortFile";
            this.SortFile.Size = new System.Drawing.Size(197, 22);
            this.SortFile.Text = "Сортировка";
            // 
            // SortAscend
            // 
            this.SortAscend.Name = "SortAscend";
            this.SortAscend.Size = new System.Drawing.Size(160, 22);
            this.SortAscend.Text = "А-Я";
            this.SortAscend.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // SortDescend
            // 
            this.SortDescend.Name = "SortDescend";
            this.SortDescend.Size = new System.Drawing.Size(160, 22);
            this.SortDescend.Text = "Я-А";
            this.SortDescend.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // NoSort
            // 
            this.NoSort.Checked = true;
            this.NoSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NoSort.Name = "NoSort";
            this.NoSort.Size = new System.Drawing.Size(160, 22);
            this.NoSort.Text = "Без сортировки";
            this.NoSort.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // ViewFile
            // 
            this.ViewFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LargeIconFile,
            this.SmallIconFile,
            this.ListFile});
            this.ViewFile.Name = "ViewFile";
            this.ViewFile.Size = new System.Drawing.Size(197, 22);
            this.ViewFile.Text = "Вид";
            // 
            // LargeIconFile
            // 
            this.LargeIconFile.Checked = true;
            this.LargeIconFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LargeIconFile.Name = "LargeIconFile";
            this.LargeIconFile.Size = new System.Drawing.Size(179, 22);
            this.LargeIconFile.Text = "Большие иконки";
            this.LargeIconFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // SmallIconFile
            // 
            this.SmallIconFile.Name = "SmallIconFile";
            this.SmallIconFile.Size = new System.Drawing.Size(179, 22);
            this.SmallIconFile.Text = "Маленькие иконки";
            this.SmallIconFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // ListFile
            // 
            this.ListFile.Name = "ListFile";
            this.ListFile.Size = new System.Drawing.Size(179, 22);
            this.ListFile.Text = "Список";
            this.ListFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // DeleteFile
            // 
            this.DeleteFile.Name = "DeleteFile";
            this.DeleteFile.Size = new System.Drawing.Size(197, 22);
            this.DeleteFile.Text = "Удалить элемент";
            this.DeleteFile.Click += new System.EventHandler(this.AddFile_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.MaximumSize = new System.Drawing.Size(50, 0);
            this.statusStrip1.MinimumSize = new System.Drawing.Size(50, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(50, 521);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Explorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.togger1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Explorer";
            this.Size = new System.Drawing.Size(860, 521);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Togger togger1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AddFile;
        private System.Windows.Forms.ToolStripMenuItem SortFile;
        private System.Windows.Forms.ToolStripMenuItem SortAscend;
        private System.Windows.Forms.ToolStripMenuItem SortDescend;
        private System.Windows.Forms.ToolStripMenuItem NoSort;
        private System.Windows.Forms.ToolStripMenuItem ViewFile;
        private System.Windows.Forms.ToolStripMenuItem LargeIconFile;
        private System.Windows.Forms.ToolStripMenuItem SmallIconFile;
        private System.Windows.Forms.ToolStripMenuItem ListFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        //Получаем элементы интерфейса, что бы их можно было изменять
        public ListView listView { get { return listView1; } }
        public TreeView treeView { get { return treeView1; } }
        public Togger togger { get { return togger1; } }
        public ToolStrip toolStrip { get { return toolStrip1; } }
        public StatusStrip statusStrip { get { return statusStrip1; } }
        public ContextMenuStrip contextMenuStrip { get { return contextMenuStrip1; } }

        DirectoryInfo direct;
        Properties.Settings set = new Properties.Settings();
        Dictionary<string, View> views = new Dictionary<string, View>() { { "LargeIconFile", View.LargeIcon }, { "SmallIconFile", View.SmallIcon }, { "ListFile", View.List } };
        ToolStripMenuItem[] sort;
        ToolStripMenuItem[] file;
        Dictionary<string, string> tree;
        Dictionary<string, List<string>> checkFile;
        Dictionary<string, SortOrder> sorts;
        public ToolStripMenuItem DeleteFile;
    }
}
