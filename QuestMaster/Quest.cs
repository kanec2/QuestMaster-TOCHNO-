using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestMaster
{
    class Quest : BaseElement, XmlSerializable
    {
        public TasksList tasks = new TasksList();
        public Quest()
        {
            tasks = new TasksList();
        }

        public Quest(string id, List<TaskElement> tasks, string typeTask) : base(id)
        {
            this.tasks = new TasksList(tasks, typeTask);
        }

        public override XElement GetXML()
        {
            XElement Xquest = base.GetXML();
            Xquest.Name = "quest";
            XElement Xtask = tasks.GetXML();
            Xquest.Add(Xtask);
            return Xquest;
        }
        public override void LoadXml(XElement xElement)
        {
            base.LoadXml(xElement);
            tasks.loadTasks(xElement.Elements("task"));
        }
        
    }
    class QuestController : BaseList<Quest>
    {
        QuestMaster.Properties.Settings set = new Properties.Settings();
        public QuestController(List<Quest> questList) : base(questList)
        {

        }
        public QuestController()
        {

        }
        public void createQuest()
        {
            list.Add(new Quest());
            moveNext();
        }
        public void addTask(TaskElement elem)
        {
            getItem().tasks.add(elem);
        }
        public void addAnswer(AnswerElement elem)
        {
            getItem().tasks.getItem().answers.add(elem);
        }
        public void saveQuest()
        {
            XDocument doc = new XDocument();
            doc.Add(GetXML());
        }
        public void loadQuests()
        {
            DirectoryInfo questArchive = new DirectoryInfo(set.QuestArchive);
            XDocument doc;
            foreach (FileInfo file in questArchive.GetFiles())
            {
                doc = XDocument.Load(file.FullName);
                createQuest();
                getItem().LoadXml(doc.Elements("quest").Single());
            }
        }
    }

}









    /*
    List<TasksElement> tasksElement;
    TasksElement elem;
    XDocument doc;
    List<string> types = new List<string>();
    int idQuest = 1;
    int idTask = 1;
    string fileName;
    QuestMaster.Properties.Settings set = new Properties.Settings();
    DirectoryInfo questArchive;
    private string idSelectedquest;

    public Quests()
    {
        quests = new Dictionary<string, List<TasksElement>>();
        questArchive = new DirectoryInfo(set.QuestArchive);

        foreach (FileInfo file in questArchive.GetFiles())
        {
            tasksElement = new List<TasksElement>();
            doc = XDocument.Load(file.FullName);
            foreach (XElement node in doc.Elements("quest"))
            {

                string typeTasks = node.FirstAttribute.Value;
                string id = node.LastAttribute.Value;
                string pathToTask = node.Element("task").FirstAttribute.Value;
                string typeAnswer = node.Element("answer").FirstAttribute.Value;
                Dictionary<string, string> answers = new Dictionary<string, string>();

                foreach (XElement answer in node.Elements("answers"))
                {
                    answers.Add(answer.FirstAttribute.Value, answer.Value);
                }
                tasksElement.Add(new TasksElement(typeTasks, pathToTask, typeAnswer, answers, int.Parse(id)));

            }

            quests.Add(file.Name.Remove(0, 5), tasksElement);

            if (this.idQuest < int.Parse(file.Name.Remove(0, 5)))
            {
                this.idQuest = int.Parse(file.Name.Remove(0, 5));
            }
        }
    }
    /// <summary>
    /// Создание Нового файла."Quest"+id 
    /// </summary>
    public void Create()
    {
        tasksElement = new List<TasksElement>();

        this.set = new Properties.Settings();
        this.doc = new XDocument(new XElement("quest", new XAttribute("id", this.idQuest), new XAttribute("idTask", idTask)));
        this.doc.Save(set.QuestArchive + "Quest" + this.idQuest);

        quests.Add(idQuest.ToString(), tasksElement);
        this.idQuest++;

    }

    /// <summary>
    /// Добавление задания с одним вариантом ответа.
    /// </summary>
    /// <param name="typeTask">Тип задания: images, text, audious, videos</param>
    /// <param name="pathToTask">Имя файла с текстом задания.</param>
    /// <param name="typeAnswers">Тип ответа.Тип ответа.toggles, variants,text,images</param>
    /// <param name="pathToAnswer">Путь до файлов с контентом.Если "text", то null</param>
    /// <param name="rightAnswer">Номер правильного варианта ответа, начиная с 0.Если "text", то null</param>
    public void Add(string typeTask, string pathToTask, string typeAnswers, List<string> pathToAnswer, string rightAnswer)
    {
        TasksElement newTask = new TasksElement(typeTask, typeAnswers, pathToTask, pathToAnswer, rightAnswer, idTask++);
        tasksElement.Add(newTask);
    }

    /// <summary>
    /// Добавление элемента с несколькими вариантоми ответов.
    /// </summary>
    /// <param name="typeTask">Тип задания: images, text, toogles, variants</param>
    /// <param name="pathToTask">Имя файла с текстом задания.</param>
    /// <param name="typeAnswers">Тип ответа.toggles, variants,text,images</param>
    /// <param name="pathToAnswer">путь до файлов с контентом.</param>
    /// <param name="rightAnswers">Лист с правильными ответами, начиная с 0</param>
    public void Add(string typeTask, string pathToTask, string typeAnswers, List<string> pathToAnswer, List<string> rightAnswers)
    {
        TasksElement newTask = new TasksElement(typeTask, typeAnswers, pathToTask, pathToAnswer, rightAnswers, idTask++);
        tasksElement.Add(newTask);
    }

    /// <summary>
    /// Изменить тип задания.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    /// <param name="typeTasks">Новый тип задания.</param>
    public void Insert(string taskId, string typeTasks)
    {
        elem = tasksElement.Where(t => t.idTask.ToString() == taskId).Single();
        elem.typeTask = typeTasks;
    }

    /// <summary>
    /// Изменить путь до файла или тип ответа.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    /// <param name="taskOrAnswers">Что изменяем: task,answers.</param>
    /// <param name="type">Новое значение для type или text.</param>
    public void Insert(string taskId, string taskOrAnswers, string type)
    {
        elem = tasksElement.Where(t => t.idTask.ToString() == taskId).Single();

        switch (taskOrAnswers)
        {
            case "task":
                elem.typeTask = type;
                break;
            case "answers":
                elem.typeAnswer = type;
                break;
        }
    }

    /// <summary>
    /// Изменяем номер правильного варианта ответа.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    /// <param name="variantAnswer">Номер правильного варианта ответа.</param>
    public void Insert(string taskId, int variantAnswer)
    {
        elem = tasksElement.Where(t => t.idTask.ToString() == taskId).Single();
        elem.answers.Values.ToList().ForEach(answer => answer = "0");
        elem.answers.Values.ToList()[variantAnswer] = "1";
    }

    /// <summary>
    /// Изменяем несколько правильных вариантов ответа.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    /// <param name="variantsAnswer">Лист правильных вариантов ответа.</param>
    public void Insert(string taskId, List<string> variantsAnswers)
    {
        elem = tasksElement.Where(t => t.idTask.ToString() == taskId).Single();
        Dictionary<string, string> temp = elem.answers;

        for (int i = 0; i < variantsAnswers.Count; i++)
        {
            elem.answers.Select(a => a = new KeyValuePair<string, string>(temp.ElementAt(i).Key, variantsAnswers[i]));
        }
    }

    /// <summary>
    /// Изменить путь для content.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    /// <param name="variantAnswers">Номер варианта ответа.</param>
    /// <param name="contentValue">Значение выбранного варианта.</param>
    public void Insert(string taskId, int variantAnswers, string contentValue)
    {
        elem = tasksElement.Where(t => t.idTask.ToString() == taskId).Single();
        elem.answers.Values.ToList()[variantAnswers] = contentValue;
    }

    /// <summary>
    /// Изменить пути для нескольких content.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    /// <param name="variantAnswers">Номера какие варианты поменять.</param>
    /// <param name="contentValue">Лист с путями до файлов.</param>
    public void Insert(string taskId, int[] variantAnswers, List<string> contentValue)
    {
        elem = tasksElement.Where(t => t.idTask.ToString() == taskId).Single();
        Dictionary<string, string> temp = elem.answers;

        for (int i = 0; i < variantAnswers.Length; i++)
        {
            elem.answers.Select(a => a = new KeyValuePair<string, string>(variantAnswers[i].ToString(), temp.ElementAt(i).Value));
        }
    }

    /// <summary>
    /// Удаляет задание.
    /// </summary>
    /// <param name="taskId">ID задания.</param>
    public void Delete(string taskId)
    {
        tasksElement.RemoveAll(el => el.idTask.ToString() == taskId);
    }

    /// <summary>
    /// Сохраняет все документы.
    /// </summary>
    public void SaveAll()
    {
        foreach (KeyValuePair<string, List<TasksElement>> quest in quests)
        {
            this.doc = XDocument.Load(set.QuestArchive + "Quest" + quest.Key);
            doc.Element("quest").RemoveNodes();
            XElement task = new XElement("task");
            foreach (TasksElement elem in quest.Value)
            {
                task = new XElement("task", new XAttribute("type", elem.typeTasks), new XAttribute("id", elem.idTask),
                    new XElement("task", new XAttribute("type", elem.typeTask)),
                    new XElement("answers", new XAttribute("type", elem.typeAnswer)));
                foreach (KeyValuePair<string, string> answer in elem.answers)
                {
                    task.Element("answers").Add(new XElement("answer", new XAttribute("content", answer.Key), new XText(answer.Value)));
                }
            }
            doc.Element("quest").Add(task);
            this.doc.Save(set.QuestArchive + "Quest" + quest.Key);
        }


    }

    /// <summary>
    /// Сохраняет последний квест.
    /// </summary>
    public void SaveElement()
    {
        this.doc = XDocument.Load(set.QuestArchive + "Quest" + idSelectedquest);
        doc.Element("quest").RemoveNodes();
        XElement task = new XElement("task");
        foreach (TasksElement elem in tasksElement)
        {
            task = new XElement("task", new XAttribute("type", elem.typeTasks), new XAttribute("id", elem.idTask),
                new XElement("task", new XAttribute("type", elem.typeTask)),
                new XElement("answers", new XAttribute("type", elem.typeAnswer)));
            foreach (KeyValuePair<string, string> answer in elem.answers)
            {
                task.Element("answers").Add(new XElement("answer", new XAttribute("content", answer.Key), new XText(answer.Value)));
            }
        }
        doc.Element("quest").Add(task);
        this.doc.Save(set.QuestArchive + "Quest" + idSelectedquest);
    }

    /// <summary>
    /// Загругрузка Квеста.
    /// </summary>
    /// <param name="id">ID Квеста.</param>
    public void Load(string id)
    {
        tasksElement = quests[id];
        idSelectedquest = id;
    }

    /// <summary>
    /// Удаление элемента в квесте.
    /// </summary>
    /// <param name="id">ID удаляемого элемента.</param>
    public void DeleteElement(string id)
    {
        tasksElement.ToList().ForEach(el =>
        {
            if (el.typeTask == id) el.typeTask = " ";
            el.answers.Keys.Where(i => i == id).Single().Remove(0, id.Count());
        });
    }

    /// <summary>
    /// Производим переиндексацию в xml документе.
    /// </summary>
    /// <param name="id">ID документа который требуется проиндексировать.</param>
    public void ReIndex(string id)
    {
        int idt = 1;
        tasksElement.ForEach(f => { f.idTask = idt; idt++; }); ;
    }

    /// <summary>
    /// Получаем все файлы используемые в квесте.
    /// </summary>
    /// <param name="path">Путь до файла.</param>
    /// <returns>List с файлами</returns>
    public List<string> GetAllFiles(string path)
    {
        XDocument template = XDocument.Load(path);
        List<string> files = new List<string>();
        foreach (TasksElement elem in tasksElement)
        {
            files.Add(elem.typeTask);
            foreach (KeyValuePair<string, string> answer in elem.answers)
            {
                files.Add(answer.Key);
            }
        }
        return files;
    }*/



