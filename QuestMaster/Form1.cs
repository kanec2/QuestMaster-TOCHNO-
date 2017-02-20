using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace QuestMaster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BDConnection connectionBD = new BDConnection("127.0.0.1", "root", "3306", "", "mydb");
            XmlDocument xmlMap = new XmlDocument();
            xmlMap.Load("file:///QuestMaster//Resources//XmlMap.xml");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            XmlDocument xmlQuest = new XmlDocument();
            
        }
    }
}
