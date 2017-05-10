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
        ModelExplorer questModelExplorer;
        ModelExplorer elementModelExplorer;
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
            questModelExplorer = new ModelExplorer(questExplorer);
            questExplorer.treeView.NodeMouseClick += mouseClickNode;
            elementModelExplorer = new ModelExplorer(elementExplorer, files, resource.getAllTags());
            elementExplorer.treeView.NodeMouseClick += mouseClickNode;
            elementModelExplorer.OnFileDelete += updateElements;
            exp3 = new ModelExplorer(explorer3);
            
        }

        private void updateElements(object sender, DeleteEventArgs e)
        {
            ResourceElement elem = resource.checkElement(e.FileName);
            List<string> buf = elem.resourceTags.tags.Where(t => t.Contains("quest")).ToList();
            string id;
            foreach (string item in buf)
            {
                id = item.Split(':')[1];
                quests.Load(id);
                quests.DeleteElement(e.Id.ToString());
                quests.Save();
            }
        }

        private void mouseClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {

            TreeNode newSelected = e.Node;
            string path = " ";
            switch(metroTabControl1.SelectedIndex)
            {
                case 1:
                    questExplorer.listView.ContextMenuStrip = questExplorer.contextMenuStrip;
                    direct = new DirectoryInfo(set.QuestArchive);
                    List<string> allFilesQuest = quests.GetAllFiles(direct.GetFiles().Where(f => f.Name == newSelected.Name).Single().FullName);
                    List<ResourceElement> questElems = resource.GetElementByID(allFilesQuest);
                    questModelExplorer.files.Clear();
                    questModelExplorer.AddListImg(imageListIconForMaterialsListView);
                    foreach (ResourceElement questElem in questElems)
                    {
                        questModelExplorer.files.Add(new CustomFile(questElem.respath, questElem.respath.Split('.')[1], questElem));
                    }
                    questModelExplorer.files.ForEach(t => t.filter(this.questModelExplorer.tags));
                    questModelExplorer.makeFiles();
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
                    elementExplorer.listView.ContextMenuStrip = elementExplorer.contextMenuStrip;
                    elementModelExplorer.files.Clear();
                    direct.GetFiles().ToList().ForEach(t => elementModelExplorer.files.Add(new CustomFile(t.Name, t.Extension, resource.checkElement(t.Name))));
                    elementModelExplorer.files.ForEach(t => t.filter(this.elementModelExplorer.tags));
                    elementModelExplorer.AddListImg(imageListIconForMaterialsListView);
                    elementModelExplorer.makeFiles();
                    break;
                case 3:
                    explorer3.listView.ContextMenuStrip = questExplorer.contextMenuStrip;
                    break;
            }
        }

        private void metroTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 1: questModelExplorer.makeTree("quest"); questModelExplorer.menu(e.TabPageIndex); break;
                case 2: elementModelExplorer.makeTree("resource"); elementModelExplorer.menu(e.TabPageIndex); break;
                case 3: exp3.makeTree("player"); exp3.menu(e.TabPageIndex); break;
            }
        }
        

    }
}
