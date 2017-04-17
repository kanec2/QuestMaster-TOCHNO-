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
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public partial class ToolStripTager : UserControl
    {
        private List<Button> tags = new List<Button>();
        public List<Button> ResTags { get { return tags; } set { tags = value; updateTags(); } }

        public event ChangedEventHandler Changed;

        protected virtual void OnTagChanged(EventArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        private void updateTags()
        {
            tags.ForEach(t => {
                flowLayoutPanel1.Controls.Add(t); t.Click += removeTag;
            });
            
        }

        public void removeTag(object sender, EventArgs e) {
            Button btn = sender as Button;
            btn.Click -= removeTag;
            flowLayoutPanel1.Controls.Remove(btn);
            ResTags.Remove(btn);
            OnTagChanged(EventArgs.Empty);
        }
        public void addTag(string tagName) {
            if(ResTags.Count !=0) if (ResTags.Select(t => t.Text == tagName).Any(v=> v== true)) return;
            Button btn = new Button()
            {
                Name = tagName,
                Size = new System.Drawing.Size(40, 23),
                Text = tagName,
                AutoSize = true
            };
            btn.Click += removeTag;
            flowLayoutPanel1.Controls.Add(btn);
            ResTags.Add(btn);
            OnTagChanged(EventArgs.Empty);
        }
        public ToolStripTager()
        {
            InitializeComponent();
        }

        public void fillTags(List<string> tagList)
        {
            comboBox1.Items.AddRange(tagList.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResTags.ForEach(t =>
            {
                flowLayoutPanel1.Controls.Remove(t); t.Click -= removeTag;
            });
            ResTags = new List<Button>();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;
            
            addTag(comboBox1.Text);
        }
    }
}
