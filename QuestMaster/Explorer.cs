using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using System.IO;

namespace QuestMaster
{
    
    
    public partial class Explorer : MetroUserControl
    {
        
        public Explorer()
        {
            InitializeComponent();
            file = new ToolStripMenuItem[] { this.LargeIconFile, this.SmallIconFile, this.ListFile };
            sort = new ToolStripMenuItem[] { this.SortAscend, this.SortDescend, this.NoSort };
            sorts = new Dictionary<string, SortOrder>() { { "SortAscend", SortOrder.Ascending }, { "SortDescend", SortOrder.Descending }, { "NoSort", SortOrder.None } };
            tree = new Dictionary<string, string>() { { "Images", set.Images }, { "Videos", set.Videos }, { "Audios", set.Audios }, { "Text", set.Text } };
            checkFile = new Dictionary<string, List<string>>() { { "Images", new List<string>() { ".jpg", ".png", ".jpeg" } }, { "Videos", new List<string>() { ".mp4" } }, { "Audios", new List<string>() { ".mp3" } }, { "Text", new List<string>() { ".txt" } } };
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            switch (clickedItem.Name)
            {
                case "AddFile":
                    direct = new DirectoryInfo(tree[this.treeView1.SelectedNode.Name]);
                    if (!(this.openFileDialog1.ShowDialog() == DialogResult.OK)) return;
                    if (direct.GetFiles().Select(t => t.Name == openFileDialog1.SafeFileName).First())
                    {
                        MessageBox.Show("Данный файл уже добавлен"); return;
                    }
                    MessageBox.Show(this.openFileDialog1.SafeFileName.Split('.')[1].ToString());
                    if (!checkFile[this.treeView1.SelectedNode.Name].Contains(this.openFileDialog1.SafeFileName.Split('.')[1]))
                    {
                        MessageBox.Show("Вы не можите добавить файл. Не соответствие формата файлов. Или неправильное имя.");
                        return;
                    }
                    File.Copy(openFileDialog1.FileName, direct.FullName + "//" + openFileDialog1.SafeFileName);
                    break;
                case "DeleteFile":
                    ListViewItem item = listView.SelectedItems[0];
                    
                    
                    
                    break;
                case "SortAscend":
                case "SortDescend":
                case "NoSort":
                    checkClicked(clickedItem);
                    listView1.Sorting = sorts[clickedItem.Name];
                    break;
                case "LargeIconFile":
                case "SmallIconFile":
                case "ListFile":
                    checkClicked(clickedItem);
                    listView1.View =  views[clickedItem.Name];
                    break;
            }
        }

        private void checkClicked(ToolStripMenuItem clickedItem) {
            foreach (ToolStripMenuItem item in sort)
            {
                item.Checked = false;
                item.CheckState = CheckState.Unchecked;
            }

            clickedItem.CheckState = CheckState.Checked;
        }
    }
    
}
