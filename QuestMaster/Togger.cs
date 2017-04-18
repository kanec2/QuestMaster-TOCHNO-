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
    
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public partial class Togger : MetroUserControl
    {
        public MetroButton[] tags;
        public int sizeing { get { return metroButton1.Height = metroComboBox1.Height; } }
        public event ChangedEventHandler Changed;


        protected virtual void OnTagChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        private void updateTags()
        {
            tags.Where(t=>t!=null).ToList().ForEach(t => {
                flowLayoutPanel1.Controls.Add(t); t.Click += removeTag;
            });

        }

        public void removeTag(object sender, EventArgs e)
        {
            MetroButton btn = sender as MetroButton;
            btn.Click -= removeTag;
            flowLayoutPanel1.Controls.Remove(btn);
            tags = tags.Where(t => t != btn).ToArray();
            OnTagChanged(EventArgs.Empty);
        }
        public void addTag(string tagName)
        {
            if (tags.ToList().Count != 0) if (tags.Where(t=>t!=null).Select(t => t.Text == tagName).Any(v => v == true)) return;
            MetroButton btn = new MetroButton()
            {
                Name = tagName,
                Size = new System.Drawing.Size(40, 23),
                Text = tagName,
                AutoSize = true
            };
            btn.Click += removeTag;
            flowLayoutPanel1.Controls.Add(btn);
            var list = new List<MetroButton>(tags);
            list.Add(btn);
            tags = list.ToArray();
            OnTagChanged(EventArgs.Empty);
        }

        public void fillTags(List<string> tagList)
        {
            metroComboBox1.Items.AddRange(tagList.ToArray());
            tags = new MetroButton[tagList.Count];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tags.Where(t=>t!=null).ToList().ForEach(t =>
            {
                flowLayoutPanel1.Controls.Remove(t); t.Click -= removeTag;
            });
            tags = new MetroButton[metroComboBox1.Items.Count];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox1.SelectedIndex == -1) return;

            addTag(metroComboBox1.Text);
        }

        public Togger()
        {
            InitializeComponent();

        }
    }
}
