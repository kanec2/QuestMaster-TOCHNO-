using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestMaster
{
    public partial class RenameDialogBox : Form
    {
        public string resultText { get { return textBox1.Text; } }

        public RenameDialogBox()
        {
            InitializeComponent();
        }
    }
}
