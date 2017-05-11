using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestMaster
{
    class Quests
    {
        XDocument doc;
        List<string> types = new List<string>();
        int idQuest = 1;
        int idTask = 1;
        string fileName;
        QuestMaster.Properties.Settings set = new Properties.Settings();
        DirectoryInfo questArchive;

        public Quests()
        {
            questArchive = new DirectoryInfo(set.QuestArchive);
            foreach (FileInfo item in questArchive.GetFiles())
            {
                if (this.idTask < int.Parse(item.Name.Remove(0, 5)))
                {
                    this.idTask = int.Parse(item.Name.Remove(0, 5));
                }
            }
        }
        /// <summary>
        /// Создание Нового файла."Quest"+id 
        /// </summary>
        public void Create()
        {
            this.set = new Properties.Settings();
            this.doc = new XDocument(new XElement("quest", new XAttribute("id", this.idQuest), new XAttribute("idTask",idTask)));
            this.doc.Save(set.QuestArchive + "Quest" + this.idQuest);
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
            XElement tasks = new XElement("tasks", new XAttribute("type", typeTask), new XAttribute("id", this.idTask));
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
            doc.Element("quest").LastAttribute.Value = idTask.ToString();
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
            XElement tasks = new XElement("tasks", new XAttribute("type", typeTask), new XAttribute("id", idTask));
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
            doc.Element("quest").LastAttribute.Value = idTask.ToString();
        }

        /// <summary>
        /// Изменить тип задания.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="typeTasks">Новый тип задания.</param>
        public void Insert(string taskId, string typeTasks)
        {
            this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().FirstAttribute.Value = typeTasks;
        }

        /// <summary>
        /// Изменить путь до файла или тип ответа.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="taskOrAnswers">Что изменяем: task,answers.</param>
        /// <param name="type">Новое значение для type или text.</param>
        public void Insert(string taskId, string taskOrAnswers, string type)
        {
            switch (taskOrAnswers)
            {
                case "task": this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Element("task").FirstAttribute.Value = type; break;
                case "answers": this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Element("answers").FirstAttribute.Value = type; break;
            }
        }

        /// <summary>
        /// Изменяем номер правильного варианта ответа.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="variantAnswer">Номер правильного варианта ответа.</param>
        public void Insert(string taskId, int variantAnswer)
        {
            for (int i = 0; i < 4; i++)
            {
                this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(i).Value = "0";
            }
            this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(variantAnswer).Value = "1";
        }

        /// <summary>
        /// Изменяем несколько правильных вариантов ответа.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="variantsAnswer">Лист правильных вариантов ответа.</param>
        public void Insert(string taskId, List<string> variantsAnswers)
        {
            for (int i = 0; i < variantsAnswers.Count; i++)
            {
                this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(i).Value = variantsAnswers[i];
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
            this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(variantAnswers).FirstAttribute.Value = contentValue;
        }

        /// <summary>
        /// Изменить пути для нескольких content.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="variantAnswers">Номера какие варианты поменять.</param>
        /// <param name="contentValue">Лист с путями до файлов.</param>
        public void Insert(string taskId, int[] variantAnswers, List<string> contentValue)
        {
            for (int i = 0; i < variantAnswers.Length; i++)
            {
                this.doc.Element("quest").Elements().Where(elem => elem.LastAttribute.Value == taskId).First().Elements("answers").Elements().ElementAt(variantAnswers[i]).FirstAttribute.Value = contentValue[i];
            }
        }

        /// <summary>
        /// Удаляет задание.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        public void Delete(string taskId)
        {
            foreach (XElement elem in this.doc.Elements("quest").Elements())
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    elem.Remove();
                }
            }
        }

        /// <summary>
        /// Сохраняем документ.
        /// </summary>
        /// <param name="fileName">Путь до файла.</param>
        public void Save()
        {
            this.doc.Save(set.QuestArchive + "Quest" + this.doc.Element("quest").FirstAttribute.Value);
        }
        
        /// <summary>
        /// Загрузка Квеста.
        /// </summary>
        /// <param name="id">ID Квеста.</param>
        public void Load(string id)
        {
            questArchive = new DirectoryInfo(set.QuestArchive);
            fileName = questArchive.GetFiles().Where(file => file.Name.Remove(0,5) == id).First().FullName;
            this.doc = XDocument.Load(fileName);
            idTask = int.Parse(this.doc.Element("quest").LastAttribute.Value);
        }
        public void DeleteElement(string id)
        {
            foreach (XElement tasks in doc.Elements("quest"))
            {
                foreach (XElement task in tasks.Elements("tasks").Elements())
                {
                    if (task.LastAttribute.Value == id) tasks.Element(task.Name).Remove();
                    task.Elements("answers").Where(f => f.FirstAttribute.Value == id).Single().Remove();
                }
            }
        }
        /// <summary>
        /// Удаление элемента в квесте.
        /// </summary>
        /// <param name="id">ID удаляемого элемента.</param>
        public void DeleteElement(string id)
        {
            foreach (XElement tasks in doc.Elements("quest"))
            {
                foreach (XElement task in tasks.Elements("tasks").Elements())
                {
                    if (task.LastAttribute.Value == id) tasks.Element(task.Name).Remove();
                    task.Elements("answers").Where(f => f.FirstAttribute.Value == id).Single().Remove();
                }
            }
        }

        /// <summary>
        /// Производим переиндексацию в xml документе.
        /// </summary>
        /// <param name="id">ID документа который требуется проиндексировать.</param>
        public void ReIndex(string id)
        {
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
