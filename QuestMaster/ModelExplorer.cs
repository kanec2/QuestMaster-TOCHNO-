using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestMaster
{
    class ModelExplorer
    {
        public List<CustomFile> files;
        Explorer explore;
        public List<string> tags;
        DBConnection dbConnect;
        Properties.Settings set = new Properties.Settings();
        DirectoryInfo dir;

#region Блок конструкторов класса
        /// <summary>
        /// Создание модели для Exlporer.
        /// </summary>
        /// <param name="explore">Explorer для работы.</param>
        public ModelExplorer(Explorer explore)
        {
            this.explore = explore;
            this.files = new List<CustomFile>();
            this.tags = new List<string>();
        }
        
        /// <summary>
        /// Создание модели для Exlporer.
        /// </summary>
        /// <param name="explore">Explorer для работы.</param>
        /// <param name="files">Файлы.</param>
        /// <param name="tags">Теги дял работы.</param>
        public ModelExplorer(Explorer explore, List<CustomFile> files, List<string> tags)
        {
            this.explore = explore;
            this.files = files;
            this.tags = tags;
            this.explore.togger.fillTags(tags);
            this.explore.togger.Changed += tagChanged;
            this.explore.listView.MouseClick += ListViewMouseClick;
        }

        /// <summary>
        /// Создание Explore с доступом к базе данных;
        /// </summary>
        /// <param name="explore">Элемент Explore</param>
        /// <param name="resources">Ресурсы</param>
        /// <param name="dbConnect">Соединение с Базой данных</param>
        public ModelExplorer(Explorer explore, List<CustomFile> files, List<string> tags, DBConnection dbConnect):this(explore,files,tags)
        {
            this.dbConnect = dbConnect;
        }

        /// <summary>
        /// Создаёт Explore, со всеми компонентами.
        /// </summary>
        /// <param name="explore">Элемент Explore.</param>
        /// <param name="resources">Ресурсы</param>
        /// <param name="state">Состояние: status, tool.</param>
        /// <param name="item">Элементы ToolStrip.</param>
        /// <param name="tags">Теги в Togger</param>
        /// <param name="tree">Узлы в дерево TreeView</param>
        public ModelExplorer(Explorer explore, List<CustomFile> files, List<string> tags, string state, ToolStripItem item, TreeNode tree) : this(explore, files, tags)
        {
            AddStripElements(item, state);
            AddTreeViewElements(tree);
        }

        /// <summary>
        /// Создаёт Explore, с Strip panel.
        /// </summary>
        /// <param name="explore">Элемент Explore.</param>
        /// <param name="resources">Ресурсы</param>
        /// <param name="state">Состояние: status, tool.</param>
        /// <param name="item">Элементы ToolStrip.</param>
        /// <param name="tags">Теги в Togger</param>
        public ModelExplorer(Explorer explore, List<CustomFile> files, List<string> tags, string state, ToolStripItem item) : this(explore, files, tags)
        {
            AddStripElements(item, state);
        }

        /// <summary>
        /// Создаёт Explore, с TreeView
        /// </summary>
        /// <param name="explore">Элемент Explore.</param>
        /// <param name="resources">Ресурсы</param>
        /// <param name="tree">Узлы в дерево TreeView</param>
        public ModelExplorer(Explorer explore, List<CustomFile> files, List<string> tags, TreeNode tree) : this(explore, files, tags)
        {
            AddTreeViewElements(tree);
        }
#endregion
        /// <summary>
        /// Добавляет текст в StatusStrip.
        /// </summary>
        /// <param name="text">Добавляемый текст.</param>
        /// <param name="state">Состояние: status, tool</param>
        public void AddStripElements(string text, string state)
        {
            switch (state)
            {
                case "status": this.explore.statusStrip.Items.Add(text); break;
                case "tool": this.explore.toolStrip.Items.Add(text); break;
            }
        }

        /// <summary>
        /// Добавляет элементы в StatusStrip
        /// </summary>
        /// <param name="item">Элементы ToolStrip</param>
        /// <param name="state">Состояние: status, tool</param>
        public void AddStripElements(ToolStripItem item,string state)
        {
            switch (state)
            {
                case "status": this.explore.statusStrip.Items.Add(item); break;
                case "tool": this.explore.toolStrip.Items.Add(item); break;
            }
        }

        /// <summary>
        /// Добавляет элементы в TreeView
        /// </summary>
        /// <param name="treeNode">Добовляемые узлы.</param>
        public void AddTreeViewElements(TreeNode treeNode)
        {
            this.explore.treeView.Nodes.Add(treeNode);
        }

        /// <summary>
        /// Добаляет теги в Togger.
        /// </summary>
        /// <param name="tags">Лист с добовляемыми тегами.</param>
        public void AddToggerTags(List<string> tags)
        {
            tags.ForEach(t => this.explore.togger.addTag(t));
        }

        /// <summary>
        /// Добаляет теги в Togger.
        /// </summary>
        /// <param name="tags">Добавляемый тег.</param>
        public void AddToggerTags(string tags)
        {
            this.explore.togger.addTag(tags);
        }


        // Событие при клике по элементу в ViewList
        private void ListViewMouseClick(object sender, MouseEventArgs e)
        {
            clearTags();
            ResourceElement elem = files[this.explore.listView.FocusedItem.Index].elem;

            if (elem == null) return;

            elem.resourceTags.tags.ForEach(t => explore.statusStrip.Items.Add(t));
        }

        // Событие для работы с тэгами в Togger
        private void tagChanged(object sender, EventArgs e)
        {
            this.tags = explore.togger.tags.Where(t => t != null).Select(t => t.Text).ToList();
            files.ForEach(t => t.filter(tags));
            makeFiles();
        }

        /// <summary>
        /// Создание элементов в ListView
        /// </summary>
        public void makeFiles()
        {
            this.explore.listView.Items.Clear();
            
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (CustomFile file in files.Where(t => t.visible == true))
            {
                item = new ListViewItem(file.fileName, file.indImage);

                item.BackColor = file.fileColor;

                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File") };

                item.SubItems.AddRange(subItems);
                this.explore.listView.Items.Add(item);
            }
        }

        /// <summary>
        /// Построение TreeView.
        /// </summary>
        /// <param name="state">Что строим: resource, quest</param>
        public void makeTree(string state)
        {
            explore.treeView.Nodes.Clear();
            TreeNode tNode = new TreeNode();

            switch(state)
            {
                case "resource":
                    this.explore.treeView.Nodes.Add("images", "Картинки");
                    this.explore.treeView.Nodes.Add("videos", "Видео");
                    this.explore.treeView.Nodes.Add("audios", "Аудио");
                    this.explore.treeView.Nodes.Add("text", "Текст");
                    break;
                case "quest":
                    dir = new DirectoryInfo(set.QuestArchive);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        this.explore.treeView.Nodes.Add(file.Name, "Квест " + file.Name.Remove(0, 5));
                    }
                    break;
                case "player":
                    break;
            }
        }

        // Очищает теги
        private void clearTags()
        {
            this.explore.statusStrip.Items.Clear();
            this.explore.statusStrip.Items.Add("Теги:");
        }
        /// <summary>
        /// Добавляем Лист с изображением.
        /// </summary>
        /// <param name="il">Лист с изображениями</param>
        public void AddListImg(ImageList il)
        {
            this.explore.listView.SmallImageList = il;
            this.explore.listView.LargeImageList = il;

        }
    }
}
