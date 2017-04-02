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
            get { return metroLabel1.Text; }
            set { metroLabel1.Text = value; }
        }
        public string TextBoxText
        {
            get { return metroTextBox1.Text; }
            set { metroTextBox1.Text = value; }
        }

        public Size textBoxSize
        {
            get
            {
                return metroTextBox1.Size = tableLayoutPanel1.MaximumSize - metroLabel1.MaximumSize;
            }
        }
        public LabeledTextBox()
        {
            InitializeComponent();
        }
    }
}
