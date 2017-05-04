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
        List<CustomFile> files = new List<CustomFile>();
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
            DBConnection connectionBD = new DBConnection("127.0.0.1", "root", "3306", "", "mydb");
            resource = new Resources();
            exp1 = new ModelExplorer(explorer1);
            explorer1.treeView.NodeMouseClick += mouseClickNode;
            exp2 = new ModelExplorer(explorer2, files, resource.getAllTags());
            explorer2.treeView.NodeMouseClick += mouseClickNode;
            exp3 = new ModelExplorer(explorer3);
            
        }

        private void mouseClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {

            TreeNode newSelected = e.Node;
            string path = " ";
            switch(metroTabControl1.SelectedIndex)
            {
                case 1:
                    explorer1.listView.ContextMenuStrip = explorer1.contextMenuStrip;
                    direct = new DirectoryInfo(set.QuestArchive);
                    List<string> allFilesQuest = quests.GetAllFiles(direct.GetFiles().Where(f => f.Name == newSelected.Name).Single().FullName);
                    List<ResourceElement> questElems = resource.GetElementByID(allFilesQuest);
                    exp1.files.Clear();
                    exp1.AddListImg(imageListIconForMaterialsListView);
                    foreach (ResourceElement questElem in questElems)
                    {
                        exp1.files.Add(new CustomFile(questElem.respath, questElem.respath.Split('.')[1], questElem));
                    }
                    exp1.files.ForEach(t => t.filter(this.exp1.tags));
                    exp1.makeFiles();
                    break;
                case 2:
                    switch (newSelected.Name)
                    {
                        case "Images": path = set.Images; break;
                        case "Videos": path = set.Videos; break;
                        case "Audios": path = set.Audios; break;
                        case "Text": path = set.Text; break;
                    }
                    direct = new DirectoryInfo(path);
                    explorer2.listView.ContextMenuStrip = explorer2.contextMenuStrip;
                    exp2.files.Clear();
                    direct.GetFiles().ToList().ForEach(t => exp2.files.Add(new CustomFile(t.Name, t.Extension, resource.checkElement(t.Name))));
                    exp2.files.ForEach(t => t.filter(this.exp2.tags));
                    exp2.AddListImg(imageListIconForMaterialsListView);
                    exp2.makeFiles();
                    break;
                case 3:
                    explorer3.listView.ContextMenuStrip = explorer1.contextMenuStrip;
                    break;
            }
        }

        private void metroTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 1: exp1.makeTree("quest"); exp1.menu(e.TabPageIndex); break;
                case 2: exp2.makeTree("resource"); exp2.menu(e.TabPageIndex); break;
                case 3: exp3.makeTree("player"); exp3.menu(e.TabPageIndex); break;
            }
        }
        

    }
}
