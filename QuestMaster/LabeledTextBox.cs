using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestMaster
{
    public partial class LabeledTextBox : UserControl
    {
        public string LabelText {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string TextBoxText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
        public LabeledTextBox()
        {
            InitializeComponent();
        }
    }
}
