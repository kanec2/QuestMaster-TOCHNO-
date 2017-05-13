using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace QuestMaster
{
    public class TaskElement :BaseElement,XmlSerializable
    {
        public string typeTask;
        public string pathToTask;
        public AnswersList answers;

        public TaskElement(string typeTask, string pathToTask,List<AnswerElement> answers,string typeAnswer, string id):base(id)
        {
            this.typeTask = typeTask;
            this.pathToTask = pathToTask;
            this.answers = new AnswersList(answers, typeAnswer);
        }
        public TaskElement() {
            typeTask = "text";
            this.answers = new AnswersList();
        }
        public override XElement GetXML()
        {
            XElement Xtask = base.GetXML();
            Xtask.Name = "task";
            Xtask.Add(new XAttribute("type", this.typeTask),new XAttribute("id",this.id), new XAttribute("content", this.pathToTask));

            XElement Xanswer = answers.GetXML();
            Xtask.Add(Xanswer);
            return Xtask;
        }
        public override void LoadXml(XElement xElement)
        {
            base.LoadXml(xElement);
            pathToTask = xElement.Attribute("content").Value;
            typeTask = xElement.Attribute("type").Value;
            answers.loadAnswers(xElement.Elements("answer"));
        }
    }

    public class TasksList : BaseList<TaskElement>
    {
        string typeTask;
        int current = 0;
        public TasksList(List<TaskElement> tasks, string typeTask) : base(tasks)
        {
            this.typeTask = typeTask;
        }
        public TasksList()
        {
            typeTask = "text";
        }
        public override XElement GetXML()
        {
            XElement Xanswers = base.GetXML();
            Xanswers.Name = "tasks";
            Xanswers.Add(new XAttribute("type", typeTask));
            return Xanswers;
        }

        internal void loadTasks(IEnumerable<XElement> enumerable)
        {
            foreach (XElement element in enumerable)
            {
                createTask();
                getItem().LoadXml(element);
            }
        }

        private void createTask()
        {
            list.Add(new TaskElement());
            moveNext();
        }
    }
}

