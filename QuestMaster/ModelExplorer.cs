using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestMaster
{
    public delegate void FileDeleteHandler(object sender, DeleteEventArgs e);

    public class DeleteEventArgs : EventArgs
    {
        private string fileName;
        private string name;
        private int id;

        public string FileName { get => fileName; }
        public string DirName { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }

        public DeleteEventArgs(string FileName)
        {
            this.fileName = FileName;
        }

        public DeleteEventArgs(string FileName, string name) : this(FileName)
        {
            this.name = name;
        }

        public DeleteEventArgs(string FileName, string name, int id) : this(FileName, name)
        {
            this.id = id;
        }
    }

    class ModelExplorer
    {
        public List<CustomFile> files;
        Explorer explore;
        public List<string> tags;
        DBConnection dbConnect;
        Properties.Settings set = new Properties.Settings();
        DirectoryInfo dir;
        ListViewItem clickedItem;
        public event FileDeleteHandler OnFileDelete;

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
            this.explore.listView.MouseDown += ListViewMouseClick;
            this.explore.DeleteFile.MouseDown += DeleteFileMouseDown;
        }

        /// <summary>
        /// Создание Explore с доступом к базе данных;
        /// </summary
        /// <param name="explore">Элемент Explore</param>
        /// <param name="resources">Ресурсы</param>
        /// <param name="dbConnect">Соединение с Базой данных</param>
        public ModelExplorer(Explorer explore, List<CustomFile> files, List<string> tags, DBConnection dbConnect) : this(explore, files, tags)
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
        public void AddStripElements(ToolStripItem item, string state)
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

        //Событие удаления элемента.
        private void DeleteFileMouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = explore.listView.SelectedItems[0];
            CustomFile file = files.Where(t => t.fileName == item.Text).Single();
            if (file.elem != null)
                OnFileDelete(this, new DeleteEventArgs(item.Text, explore.treeView.SelectedNode.Name, file.elem.id));
            File.Delete("|DataDirectory|\\Resources\\" + explore.treeView.SelectedNode.Name + "\\" + file.fileName);
        }

        //Событие происходит при выборе элемента.
        private void ListViewMouseClick(object sender, MouseEventArgs e)
        {
            clickedItem = explore.listView.GetItemAt(e.Location.X, e.Location.Y);
            switchContext(true);
            if (clickedItem == null) return;
            clearTags();
            ResourceElement elem = files.Find(f => f.fileName == clickedItem.Text).elem;

            if (elem != null) elem.resourceTags.tags.ForEach(t => explore.statusStrip.Items.Add(t));

            switchContext(false);

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

            switch (state)
            {
                case "resource":
                    this.explore.treeView.Nodes.Add("Images", "Картинки");
                    this.explore.treeView.Nodes.Add("Videos", "Видео");
                    this.explore.treeView.Nodes.Add("Audios", "Аудио");
                    this.explore.treeView.Nodes.Add("Text", "Текст");
                    break;
                case "quest":
                    dir = new DirectoryInfo(set.QuestArchive);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        this.explore.treeView.Nodes.Add(file.Name, "Квест " + file.Name.Remove(0, 5));
                    }
                    break;
                case "player":
                    this.explore.treeView.Nodes.Add("teams", "Команды");
                    this.explore.treeView.Nodes.Add("SinglePlayer", "Одиночные игроки");
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
        /// Переключение контекстного меню
        /// </summary>
        /// <param name="light">Включить или выключить.</param>
        private void switchContext(bool light)
        {
            foreach (ToolStripItem item in this.explore.contextMenuStrip.Items)
            {
                item.Visible = light;
            }
            this.explore.contextMenuStrip.Items[3].Visible = !light;

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

        /// <summary>
        /// Меняем компаненты Контекстного меню в зависимости от выбранной страцы.
        /// </summary>
        /// <param name="tabPagesIDX">Индекс страниц.</param>
        public void menu(int tabPagesIDX)
        {
            switch (tabPagesIDX)
            {
                case 1:
                case 3:
                    this.explore.contextMenuStrip.Items[0].Visible = false;
                    this.explore.contextMenuStrip.Items[3].Visible = false;
                    break;
                case 2:
                    this.explore.contextMenuStrip.Items[3].Visible = false;
                    break;


            }
        }
    }
}
