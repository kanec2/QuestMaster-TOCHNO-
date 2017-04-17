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

    public partial class ToolStripTager : UserControl
    {
        public Button[] tags;

        public event ChangedEventHandler Changed;

        protected virtual void OnTagChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        private void updateTags()
        {
            tags.ToList().ForEach(t => {
                flowLayoutPanel1.Controls.Add(t); t.Click += removeTag;
            });
            
        }

        public void removeTag(object sender, EventArgs e) {
            Button btn = sender as Button;
            btn.Click -= removeTag;
            flowLayoutPanel1.Controls.Remove(btn);

            tags = tags.Where(t => t != btn).ToArray();
            OnTagChanged(EventArgs.Empty);

        }
        public void addTag(string tagName)
        {
            if (tags.ToList().Where(t => t != null).Select(t => t.Text == tagName).Any(v => v == true)) return;

            Button btn = new Button()
            {
                Location = new System.Drawing.Point(3, 3),
                Name = tagName,
                Size = new System.Drawing.Size(50, 23),
                Text = tagName,
                AutoSize = true
            };

            btn.Click += removeTag;
            flowLayoutPanel1.Controls.Add(btn);
            var tmp = new List<Button>(tags);
            tmp.Add(btn);
            tags = tmp.ToArray();
            OnTagChanged(EventArgs.Empty);

        }

        public ToolStripTager()
        {
            InitializeComponent();

        }

        public void fillTags(List<string> tagList)
        {
            tags = new Button[tagList.Count];
            comboBox1.Items.AddRange(tagList.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tags.ToList().ForEach(t =>
            {
                flowLayoutPanel1.Controls.Remove(t); t.Click -= removeTag;
            });

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            
            addTag(comboBox1.Text);
        }
    }
}
