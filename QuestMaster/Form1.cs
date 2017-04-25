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
            
        }

        private void mouseClickNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            string path = " ";
            switch(metroTabControl1.SelectedIndex)//"Квест1" или "Картинки"
            {
                case 1:
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
                        case "images": path = set.Images; break;
                        case "videos": path = set.Videos; break;
                        case "audios": path = set.Audios; break;
                        case "text": path = set.Text; break;
                    }
                    direct = new DirectoryInfo(path);
                    exp2.files.Clear();
                    direct.GetFiles().ToList().ForEach(t => exp2.files.Add(new CustomFile(t.Name, t.Extension, resource.checkElement(t.Name))));
                    exp2.files.ForEach(t => t.filter(this.exp2.tags));
                    exp2.AddListImg(imageListIconForMaterialsListView);
                    exp2.makeFiles();
                    break;
                case 3:
                    break;
            }
        }

        private void metroTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch (e.TabPageIndex)
            {
                case 1: exp1.makeTree("quest"); break;
                case 2: exp2.makeTree("resource"); break;
                case 3: exp3.makeTree("player"); break;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.Location.ToString());
        }
    }
}
