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
        Dictionary<XName, List<ResourceElement>> resources = new Dictionary<XName, List<ResourceElement>>();
        ResourceList resList;
        public int id;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BDConnection connectionBD = new BDConnection("127.0.0.1", "root", "3306", "", "mydb");
            XDocument doc = XDocument.Load("Resources//XMLMap.xml");
            XmlWorker xWork = new XmlWorker(doc.Elements().Last());
            resList = new ResourceList();
            this.resources = xWork.resources;
            this.id = xWork.id;
            xWork.writeToXml(doc.Elements("QuestMaster").First());
            resList.add("Images", "Resources//DSC01793.jpg");
            doc.Save("Resources//XMLMap.xml");
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView2.Items.Clear();
            DirectoryInfo nodeDirInfo =  new DirectoryInfo(newSelected.Tag + " ");
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
               
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, "Directory"),
                     new ListViewItem.ListViewSubItem(item,
                        dir.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listView2.Items.Add(item);
            }

            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                int indImage = 0;
                switch(e.Node.Name)
                {
                    case "Images": indImage = 1; break;
                    case "Videos": indImage = 2; break;
                    case "Audios": indImage = 3; break;
                    case "Text": indImage = 4; break;
                }
                //resource[e.Node.Name].Select(elem => elem.);
                item = new ListViewItem(file.Name, indImage);
                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File"),
                     new ListViewItem.ListViewSubItem(item,
                        file.LastAccessTime.ToShortDateString())};

                item.SubItems.AddRange(subItems);
                listView2.Items.Add(item);
            }

            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}
