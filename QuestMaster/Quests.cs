using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestMaster
{
    struct TasksElement
    {
        public int idTask;
        public string typeTasks;
        public string typeTask;
        public string typeAnswer;
        public Dictionary<string, string> answers;

        public TasksElement(string typeTask, string pathToTask, string typeAnswer, Dictionary<string,string> answers, int id)
        {
            this.typeTasks = typeTask;
            this.typeAnswer = typeAnswer;
            this.answers = answers;
            this.idTask = id;
            this.typeTask = pathToTask;
        }

        public TasksElement(string typeTask, string pathToTask, string typeAnswer, List<string> answers, string rightAnswers, int id)
        {
            this.typeTasks = typeTask;
            this.typeAnswer = typeAnswer;
            this.answers = new Dictionary<string, string>();
            this.idTask = id;
            this.typeTask = pathToTask;

            foreach (string answer in answers)
            {
                this.answers.Add(answer,"0");
            }

            this.answers.ElementAt(int.Parse(rightAnswers));
        }
        public TasksElement(string typeTask, string typeAnswer, string pathToTask, List<string> answers, List<string> rightAnswers, int id)
        {
            this.typeTasks = typeTask;
            this.typeAnswer = typeAnswer;
            this.answers = new Dictionary<string, string>();
            this.idTask = id;
            this.typeTask = pathToTask;

            foreach (string answer in answers)
            {
                this.answers.Add(answer, rightAnswers.ElementAt(answers.IndexOf(answer)));
            }
        }

    }

    class Quests
    {
        Dictionary<string, List<TasksElement>> quests;
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
                        answers.Add(answer.FirstAttribute.Value,answer.Value);
                    }
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
            this.doc = new XDocument(new XElement("quest", new XAttribute("id", this.idQuest), new XAttribute("idTask",idTask)));
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

            /*XElement tasks = new XElement("tasks", new XAttribute("type", typeTask), new XAttribute("id", this.idTask));
            XElement task = new XElement("task", new XAttribute("text", pathToTask));
            XElement answers = new XElement("answers", new XAttribute("type", typeAnswers));

            if (typeAnswers == "text")
            {
                answers.Add(new XElement("answer", new XAttribute("content", pathToAnswer.First())));
            }
            else
            {
                foreach (string answer in pathToAnswer)
                {
                    answers.Add(new XElement("answer", new XAttribute("content", answer), new XText("0")));
                }
                answers.Elements().ElementAt(int.Parse(rightAnswer)).Value = "1";
            }

            tasks.Add(task, answers);
            doc.Element("quest").Add(tasks);
            this.idTask++;
            doc.Element("quest").LastAttribute.Value = idTask.ToString();*/
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
            /*XElement tasks = new XElement("tasks", new XAttribute("type", typeTask), new XAttribute("id", idTask));
            XElement task = new XElement("task", new XAttribute("text", pathToTask));
            XElement answers = new XElement("answers", new XAttribute("type", typeAnswers));

            if (typeAnswers == "text")
            {
                answers.Add(new XElement("answer", new XAttribute("content", pathToAnswer.First())));
            }
            else
            {
                foreach (string answer in pathToAnswer)
                {
                    answers.Add(new XElement("answer", new XAttribute("content", answer), new XText("0")));
                }
                for (int i = 0; i < 4; i++)
                {
                    answers.Elements().ElementAt(i).Value = rightAnswers[i];
                }
            }

            tasks.Add(task, answers);
            doc.Element("quest").Add(tasks);
            this.idTask++;
            doc.Element("quest").LastAttribute.Value = idTask.ToString();*/
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
            //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().FirstAttribute.Value = typeTasks;
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
                    //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Element("task").FirstAttribute.Value = type;
                    break;
                case "answers":
                    elem.typeAnswer = type;
                    //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Element("answers").FirstAttribute.Value = type;
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

            //for (int i = 0; i < 4; i++)
            //{
            //    this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(i).Value = "0";
            //}
            //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(variantAnswer).Value = "1";
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
                //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(i).Value = variantsAnswers[i];
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
            //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(variantAnswers).FirstAttribute.Value = contentValue;
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
                //this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(variantAnswers[i]).FirstAttribute.Value = contentValue[i];
            }
        }

        /// <summary>
        /// Удаляет задание.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        public void Delete(string taskId)
        {
            tasksElement.RemoveAll(el => el.idTask.ToString() == taskId);
            
            //foreach (XElement elem in this.doc.Elements("quest").Elements())
            //{
            //    if (elem.LastAttribute.Value == taskId)
            //    {
            //        elem.Remove();
            //    }
            //}
        }

        /// <summary>
        /// Сохраняет все документы.
        /// </summary>
        public void SaveAll()
        {
            foreach (KeyValuePair<string,List<TasksElement>> quest in quests)
            {
                this.doc = XDocument.Load(set.QuestArchive+"Quest" + quest.Key);
                doc.Element("quest").RemoveNodes();
                XElement task = new XElement("task");
                foreach (TasksElement elem in quest.Value)
                {
                    task = new XElement("task", new XAttribute("type", elem.typeTasks), new XAttribute("id", elem.idTask),
                        new XElement("task", new XAttribute("type", elem.typeTask)),
                        new XElement("answers", new XAttribute("type", elem.typeAnswer)));
                    foreach (KeyValuePair<string,string> answer in elem.answers)
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
            //questArchive = new DirectoryInfo(set.QuestArchive);
            //fileName = questArchive.GetFiles().Where(file => file.Name.Remove(0,5) == id).First().FullName;
            //this.doc = XDocument.Load(fileName);
            //idQuest = int.Parse(this.doc.Element("quest").LastAttribute.Value);
        }

        /// <summary>
        /// Удаление элемента в квесте.
        /// </summary>
        /// <param name="id">ID удаляемого элемента.</param>
        public void DeleteElement(string id)
        {
            tasksElement.ToList().ForEach(el => {
                if (el.typeTask == id) el.typeTask = " ";
                el.answers.Keys.Where(i => i == id).Single().Remove(0, id.Count());
            });
            //foreach (XElement tasks in doc.Elements("quest"))
            //{
            //    foreach (XElement task in tasks.Elements("tasks").Elements())
            //    {
            //        if (task.LastAttribute.Value == id) tasks.Element(task.Name).Remove();
            //        task.Elements("answers").Where(f => f.FirstAttribute.Value == id).Single().Remove();
            //    }
            //}
        }

        /// <summary>
        /// Производим переиндексацию в xml документе.
        /// </summary>
        /// <param name="id">ID документа который требуется проиндексировать.</param>
        public void ReIndex(string id)
        {
            //tasksElement.
            //***************************************************************************************************
            this.questArchive = new DirectoryInfo(set.QuestArchive);
            XDocument template = XDocument.Load(this.questArchive.GetFiles().Where(file => file.Name.Remove(0, 5) == id).First().FullName);
            int idTaks = 1;
            foreach (XElement tasks in template.Element("quest").Elements())
            {
                tasks.LastAttribute.Value = idTaks.ToString();
                idTaks++;
            }
            template.Element("quest").LastAttribute.Value = idTaks.ToString();
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
            foreach (XElement tasks in template.Elements("quest"))
            {
                foreach (XElement task in tasks.Elements("tasks").Elements())
                {
                    if (task.Name == "task") { files.Add(task.FirstAttribute.Value); }
                    if (task.Name == "answers")
                    {
                        foreach (XElement answer in task.Elements())
                        {
                            files.Add(answer.FirstAttribute.Value);
                        }
                    }
                }
            }
            return files;
        }
    }
}
