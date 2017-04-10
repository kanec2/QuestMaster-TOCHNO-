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
        QuestMaster.Properties.Settings set;

        /// <summary>
        /// Создаём новый XML файл.
        /// </summary>
        /// <param name="fileName">Путь до файла.</param>
        public Quests(string fileName)
        {
            this.fileName = fileName;
            set = new Properties.Settings();
            DirectoryInfo dirInfo = new DirectoryInfo(set.QuestArchive);
            if (dirInfo.GetFiles().Select(v => v.Name == fileName).First())
            {
                doc = XDocument.Load(dirInfo.GetFiles().Where(v => v.Name == fileName).First().FullName);
            }
            else
            {
                doc = new XDocument(new XElement("quest", new XAttribute("id", this.idQuest)));
                doc.Save(fileName);
                this.idQuest++;
            }
            
        }

        /// <summary>
        /// Добавление задания с одним вариантом ответа.
        /// </summary>
        /// <param name="typeTask">Тип задания: images, text, toogles, variants</param>
        /// <param name="pathToTask">Имя файла с текстом задания.</param>
        /// <param name="typeAnswers">Тип ответа.Тип ответа.toggles, variants,text,images</param>
        /// <param name="pathToAnswer">Путь до файлов с контентом.Если "text", то null</param>
        /// <param name="rightAnswer">Номер правильного варианта ответа, начиная с 0.Если "text", то null</param>
        public void Add(string typeTask, string pathToTask, string typeAnswers, List<string> pathToAnswer, string rightAnswer)
        {
            XElement tasks = new XElement("tasks", new XAttribute("type", typeTask), new XAttribute("id", idTask));
            XElement task = new XElement("task", new XAttribute("text", pathToTask));
            XElement answers = new XElement("answers", new XAttribute("type", typeAnswers));

            if (typeTask == "text")
            {
                answers.Add(new XElement("answer", new XAttribute("content", "inputField")));
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

            if (typeTask == "text")
            {
                answers.Add(new XElement("answer", new XAttribute("content", "inputField")));
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
        }

        /// <summary>
        /// Изменить тип задания.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="typeTasks">Тип задания на которое меняем.</param>
        public void Insert(string taskId, string typeTasks)
        {
            foreach (XElement elem in this.doc.Elements("quest"))
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    elem.FirstAttribute.Value = typeTasks;
                }
            }
        }

        /// <summary>
        /// Изменить путь до файла или тип ответа.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="taskOrAnswers">Что изменяем: task,answers.</param>
        /// <param name="type">На что меняем.</param>
        public void Insert(string taskId, string taskOrAnswers, string type)
        {
            foreach (XElement elem in this.doc.Elements("quest"))
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    switch(taskOrAnswers)
                    {
                        case "task": elem.Element("task").Value = type;  break;
                        case "answers": elem.Element("answers").Value = type; break;
                    }
                }
            }
        }

        /// <summary>
        /// Изменяем вариант правильного ответа.
        /// </summary>
        /// <param name="doc">XML Файл.</param>
        /// <param name="taskId">ID задания.</param>
        /// <param name="variantAnswer">Номер правильного варианта ответа.</param>
        public void Insert(XDocument doc, string taskId, int variantAnswer)
        {
            foreach(XElement elem in doc.Elements("quest"))
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    foreach (XElement item in elem.Elements("answers"))
                    {
                        elem.Value = "0";
                    }
                    elem.Elements("answers").ElementAt(variantAnswer).Value = "1";
                }
            }
        }

        /// <summary>
        /// Изменить несколько вариантов ответа.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="variantsAnswer">Лист правильных вариантов ответа.</param>
        public void Insert(string taskId, List<string> variantsAnswer)
        {
            foreach (XElement elem in this.doc.Elements("quest"))
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    for (int i = 0; i < variantsAnswer.Count; i++)
                    {
                        elem.Elements("answers").ElementAt(i).Value = variantsAnswer[i];
                    }
                    
                }
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
            foreach (XElement elem in this.doc.Elements("quest"))
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    elem.Elements("answers").ElementAt(variantAnswers).FirstAttribute.Value = contentValue;
                }
            }
        }

        /// <summary>
        /// Изменить пути для нескольких content.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        /// <param name="variantAnswers">Номера какие варианты поменять.</param>
        /// <param name="contentValue">Лист с путями до файлов.</param>
        public void Insert(string taskId, int[] variantAnswers, List<string> contentValue)
        {
            foreach (XElement elem in this.doc.Elements("quest"))
            {
                if (elem.LastAttribute.Value == taskId)
                {
                    for (int i = 0; i < variantAnswers.Length; i++)
                    {
                        elem.Elements("answers").ElementAt(variantAnswers[i]).FirstAttribute.Value = contentValue[i];
                    }
                    
                }
            }
        }

        /// <summary>
        /// Удаляет задание.
        /// </summary>
        /// <param name="taskId">ID задания.</param>
        public void Delete(string taskId)
        {
            foreach (XElement elem in this.doc.Elements("quest"))
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
            if (fileName.Length >0)
            {
                doc.Save(set.QuestArchive + fileName);
            }
            else
            {
                doc.Save("Resources//QuestArchive//Quest" + doc.Element("quest").FirstAttribute.Value);
            }
        }
    }
}
