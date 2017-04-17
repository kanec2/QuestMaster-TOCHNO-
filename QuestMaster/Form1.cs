using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace QuestMaster
{
    public partial class Form1 : MetroForm
    {
        Resources resource;
        public int id;
        DirectoryInfo lastDir;
        List<CustomFile> files = new List<CustomFile>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ToolStripTager tager = new ToolStripTager();
            metroTabControl1.TabPages[1].Controls.Add(tager);
            BDConnection connectionBD = new BDConnection("127.0.0.1", "root", "3306", "", "mydb");
            resource = new Resources();
            toolStripTager1.fillTags(resource.getAllTags());
            toolStripTager1.Changed += tagChanged;
        }

        private void tagChanged(object sender, EventArgs e)
        {
            List<string> tags = toolStripTager1.ResTags.Select(t => t.Text).ToList();
            files.ForEach(t => t.filter(tags));
            makeFiles();
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            lastDir = new DirectoryInfo(newSelected.Tag + " ");
            files.Clear();
            lastDir.GetFiles().ToList().ForEach(t => files.Add(new CustomFile(t.Name,t.Extension, resource.checkElement(t.Name))));
            List<string> tags = toolStripTager1.ResTags.Select(t => t.Text).ToList();
            files.ForEach(t => t.filter(null));
            makeFiles();
        }


        public void makeFiles() {
            listView2.Items.Clear();
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (CustomFile file in files.Where(t=>t.visible == true))
            {
                item = new ListViewItem(file.fileName, file.indImage);

                item.BackColor = file.fileColor;

                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File")
                    };

                    item.SubItems.AddRange(subItems);
                    listView2.Items.Add(item);
            }
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            clearTags();
            ResourceElement elem = resource.checkElement(listView2.FocusedItem.Text);
            
            if (elem == null) return;
            
            elem.resourceTags.tags.ForEach(t => statusStrip2.Items.Add(t));

        }
        private void clearTags() {
            statusStrip2.Items.Clear();
            statusStrip2.Items.Add("Теги:");
        }
    }
}
