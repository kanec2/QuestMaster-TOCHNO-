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
    public partial class LabeledComboBox : UserControl
    {
        public string[] ItemList {
            get {
                return comboBox1.Items.Cast<string>().ToArray<string>();
            }
            set {
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(value.ToArray<string>());
            } 
        }
        public string LabelText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public Size ComboBoxSize
        {
            get
            {
                return comboBox1.Size = tableLayoutPanel1.MaximumSize- label1.MaximumSize;
            }
            set
            {
                comboBox1.Size = value;
            }
        }
        public Size tableLayoutSize
        {
            get
            {
                return tableLayoutPanel1.Size = comboBox1.Size + label1.Size;
            }
            
        }

        public LabeledComboBox()
        {
            InitializeComponent();
           // comboBox1.ite
        }
    }
}
