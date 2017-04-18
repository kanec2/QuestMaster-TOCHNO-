using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestMaster
{
    public class CustomFile
    {
        public bool visible;
        public ResourceElement elem;
        public string fileName;
        public Color fileColor;
        public int indImage;
        private string extension;

        public CustomFile(string name, string extension, ResourceElement resourceElement) 
        {
            this.visible = true;
            this.fileName = name;
            this.extension = extension;
            this.elem = resourceElement;
            this.fileColor = (elem == null) ? Color.IndianRed : Color.LightGreen;
            indImage = 0;
            switch (extension)
            {
                case ".jpg": case ".png": indImage = 1; break;
                case ".mp4": indImage = 2; break;
                case ".mp3": indImage = 3; break;
                case ".txt": indImage = 4; break;
            }
        }

        internal void filter(List<string> tags)
        {
            if (elem == null || tags == null) return;

            visible = false;
            foreach(string tag in tags)
            {
                if (elem.resourceTags.tags.Contains(tag))
                {
                    visible = true;return;
                }
            }
            if (tags.Count == 0)
            {
                visible = true;
            }
        }
    }
}
