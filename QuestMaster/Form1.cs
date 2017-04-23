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
        //формирование списка для listview. в квестах. который берётся в зависимости от explorer (explorer1, explorer2 etc) 
        Resources resource;
        public int id;
        List<CustomFile> files = new List<CustomFile>();
        List<string> tags;
        ModelExplorer exp1;
        ModelExplorer exp2;
        ModelExplorer exp3;
        Properties.Settings set = new Properties.Settings();
        DirectoryInfo direct;
        Quests quests = new Quests();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Togger tager = new Togger();
            metroTabControl1.TabPages[1].Controls.Add(tager);
            DBConnection connectionBD = new DBConnection("127.0.0.1", "root", "3306", "", "mydb");
            resource = new Resources();
            exp1 = new ModelExplorer(explorer1);
            explorer1.treeView.NodeMouseClick += mouseClickNode;
            exp2 = new ModelExplorer(explorer2, files, resource.getAllTags());
            explorer2.treeView.NodeMouseClick += mouseClickNode;
            exp2.AddListImg(imageListIconForMaterialsListView);
            //ToolStripItem tsi = ToolStripItem
            //exp2.AddStripElements();
        }

        private void mouseClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            string path = " ";
            switch(sender.ToString().Remove(0, 67))//"Квест1" или "Картинки"
            {
                case "Квест 1":
                    direct = new DirectoryInfo(set.QuestArchive);
                    string texr = newSelected.Name.Remove(0, 5);
                    List<string> allFiles = quests.GetAllFiles(direct.GetFiles().Where(f => f.Name == newSelected.Name).Single().FullName);
                    //Получили файлы. теперь из них осталось построить сам listView
                    
                    break;
                case "Картинки":
                    switch (newSelected.Name)
                    {
                        case "images": path = set.Images; break;
                        case "videos": path = set.Videos; break;
                        case "audios": path = set.Audios; break;
                        case "text": path = set.Text; break;
                    }
                    direct = new DirectoryInfo(path);
                    exp2.files.Clear();
                    direct.GetFiles().ToList().ForEach(t => exp2.files.Add(new CustomFile(t.Name, t.Extension, resource.checkElement(t.Name))));
                    exp2.files.ForEach(t => t.filter(this.exp2.tags));
                    exp2.makeFiles();
                    //Починить фильтр
                    //************************************
                    break;
            }
        }

        private void metroTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 1: exp1.makeTree("quest"); break;
                case 2: exp2.makeTree("resource"); break;
              //case 3: exp.makeTree("player"); break;
            }
        }


        //public void makeFiles() {
        //    listView2.Items.Clear();
        //    ListViewItem.ListViewSubItem[] subItems;
        //    ListViewItem item = null;

        //    foreach (CustomFile file in files.Where(t=>t.visible == true))
        //    {
        //        item = new ListViewItem(file.fileName, file.indImage);

        //        item.BackColor = file.fileColor;

        //        subItems = new ListViewItem.ListViewSubItem[]
        //            { new ListViewItem.ListViewSubItem(item, "File") };

        //            item.SubItems.AddRange(subItems);
        //            listView2.Items.Add(item);
        //    }
        //}

        //private void listView2_MouseClick(object sender, MouseEventArgs e)
        //{
        //    clearTags();
        //    ResourceElement elem = resource.checkElement(listView2.FocusedItem.Text);

        //    if (elem == null) return;

        //    elem.resourceTags.tags.ForEach(t => statusStrip2.Items.Add(t));
        //}
        //private void clearTags() {
        //    statusStrip2.Items.Clear();
        //    statusStrip2.Items.Add("Теги:");
        //}
    }
}
