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

namespace QuestMaster
{
    public partial class Explorer : MetroUserControl
    {
        //Получаем элементы интерфейса, что бы их можно было изменять
        public ListView listView { get { return listView1; } set { listView = listView1; } }
        public TreeView treeView { get { return treeView1; } set { treeView = treeView1; } }
        public Togger togger { get { return togger1; } set { togger = togger1; } }
        public ToolStrip toolStrip { get { return toolStrip1; } set { toolStrip = toolStrip1; } }
        public StatusStrip statusStrip { get { return statusStrip1; } set { statusStrip = statusStrip1; } }

        public Explorer()
        {
            InitializeComponent();
        }
    }
}
