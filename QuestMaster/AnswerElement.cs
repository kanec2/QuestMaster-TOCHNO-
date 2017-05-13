using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace QuestMaster
{
    public class AnswerElement : BaseElement,XmlSerializable{
        public bool isRight;
        public AnswerElement(string id, bool isRight):base(id) {
            this.isRight = isRight;
        }
        public AnswerElement() {
            
        }
        public override XElement GetXML()
        {
            XElement Xanswer = base.GetXML();
            Xanswer.Name = "answer";
            Xanswer.Value = isRight.ToString();
            return Xanswer;
        }

        public void setParams(string id, bool isRight) {
            this.id = id;
            this.isRight = isRight;
        }

        public override void LoadXml(XElement xElement)
        {
            base.LoadXml(xElement);
            this.isRight = bool.Parse(xElement.Value);
        }

    }
    public class AnswersList:BaseList<AnswerElement> { 
        string typeAnswer;
        int current = 0;
        public AnswersList(List<AnswerElement> answers, string typeAnswer):base(answers)
        {
            this.typeAnswer = typeAnswer;
        }
        public AnswersList()
        {
            typeAnswer = "text";
        }
        public override XElement GetXML()
        {
            XElement Xanswers = base.GetXML();
            Xanswers.Name = "answers";
            return Xanswers;
        }

        internal void loadAnswers(IEnumerable<XElement> enumerable)
        {
            foreach (XElement element in enumerable)
            {
                createAnswer();
                getItem().LoadXml(element);
            }
        }

        private void createAnswer()
        {
            list.Add(new AnswerElement());
            moveNext();
        }
    }
}


