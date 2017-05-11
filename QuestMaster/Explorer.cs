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
            sorts = new Dictionary<string, SortOrder>() { { "SortAscend", SortOrder.Ascending}, { "SortDescend", SortOrder.Descending}, { "NoSort", SortOrder.None} };
            tree = new Dictionary<string, string>() { { "Images", set.Images }, { "Videos", set.Videos }, { "Audios", set.Audios }, { "Text", set.Text } };
            checkFile = new Dictionary<string, List<string>>() { { "Images", new List<string>() { ".jpg", ".png", ".jpeg" } }, { "Videos", new List<string>() { ".mp4" } }, { "Audios", new List<string>() { ".mp3" } }, { "Text", new List<string>() { ".txt" } } };
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            direct = new DirectoryInfo(tree[this.treeView1.SelectedNode.Name]);
            switch (clickedItem.Name)
            {
                case "AddFile":
                    if (!(this.openFileDialog1.ShowDialog() == DialogResult.OK)) return;
                    if (direct.GetFiles().Select(t => t.Name == openFileDialog1.SafeFileName).First())
                    {
                        MessageBox.Show("Данный файл уже добавлен"); return;
                    }
                    if (!checkFile[this.treeView1.SelectedNode.Name].Contains(this.openFileDialog1.SafeFileName.Split('.')[1]))
                    {
                        MessageBox.Show("Вы не можите добавить файл. Не соответствие формата файлов. Или неправильное имя.");
                        return;
                    }
                    File.Copy(openFileDialog1.FileName, direct.FullName + "//" + openFileDialog1.SafeFileName);
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
                case "DeleteFile":
                    if (listView1.SelectedItems.Count == 0) return;
                    foreach (ListViewItem item in this.listView1.SelectedItems)
                    {
                        File.Delete(direct.FullName + "//" + item.Text);
                        listView1.Items.Remove(item);
                    }
                    break;
                case "RenameFile":
                    dialog = new RenameDialogBox();
                    if (listView1.SelectedItems.Count == 0) return;
                    dialog.ShowDialog();
                    if(dialog.resultText == " ") return;
                    foreach (ListViewItem item in this.listView1.SelectedItems)
                    {
                        direct.GetFiles().Where(fi => fi.Name == item.Text).Single().MoveTo(direct.FullName + "//" + dialog.resultText + item.Text.Split('.')[1]);
                        item.Text = dialog.resultText;
                    }
                    break;
            }
        }

        private void checkClicked(ToolStripMenuItem clickedItem)
        {
            foreach (ToolStripMenuItem item in sort)
            {
                item.Checked = false;
                item.CheckState = CheckState.Unchecked;
            }

            clickedItem.CheckState = CheckState.Checked;
        }
    }
}
