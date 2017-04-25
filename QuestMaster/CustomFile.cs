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
        /// <summary>
        /// Фильтрует CustomFile по тегам.
        /// </summary>
        /// <param name="tags">Лист с тегами.</param>
        internal void filter(List<string> tags)
        {
            if (elem == null || tags == null) return;

            visible = false;
            foreach(string tag in tags)
            {
                switch (tag)
                {
                    case "Картинки":
                        if (indImage == 1)
                        {
                            visible = true; return;
                        }
                        break;
                    case "Видео":
                        if (indImage == 2)
                        {
                            visible = true; return;
                        }
                        break;
                    case "Аудио":
                        if (indImage == 3)
                        {
                            visible = true; return;
                        }
                        break;
                    case "Текст":
                        if (indImage == 4)
                        {
                            visible = true; return;
                        }
                        break;
                    default:
                        if (elem.resourceTags.tags.Contains(tag))
                        {
                            visible = true; return;
                        }
                        break;
                }
            }
            if (tags.Count == 0)
            {
                visible = true;
            }
        }
    }
}
