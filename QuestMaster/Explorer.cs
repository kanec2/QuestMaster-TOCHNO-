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
        public ListView listView { get { return listView1; }  }
        public TreeView treeView { get { return treeView1; }  }
        public Togger togger { get { return togger1; }  }
        public ToolStrip toolStrip { get { return toolStrip1; }  }
        public StatusStrip statusStrip { get { return statusStrip1; }  }

        public Explorer()
        {
            InitializeComponent();
        }

        private void Explorer_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show(this.GetChildAtPoint(e.Location).Name);
        }
    }
}
