using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestMaster
{
    public interface XmlSerializable
    {
        XElement GetXML();
        void LoadXml(XElement xElement);
    }

    public class BaseElement :XmlSerializable
    {
        public string id;
        public BaseElement() {
            id = "1";
        }
        public BaseElement(string id)
        {
            this.id = id;
        }

        public virtual XElement GetXML()
        {
            return new XElement("element",new XAttribute("id",this.id));
        }

        public virtual void LoadXml(XElement xElement)
        {
            this.id = xElement.Attribute("id").Value;
        }
    }
    public class BaseList<T> where T:BaseElement, XmlSerializable
    {
        public List<T> list = new List<T>();
        int current = -1;
        public BaseList(List<T> list)
        {
            this.list = list;
        }
        public BaseList()
        {

        }
        public void add(T item) {
            list.Add(item);
        }
        public bool haveNext() => (current < list.Count);
        public void moveNext()
        {
            if(current<list.Count)
            current++;
        }
        public void movePrevious()
        {
            if(current >=1)
            current--;
        }
        public T getItem() => list[current];
        public void moveFirst() => current = 0;
        public void moveLast() => current = list.Count - 1;
        
        public virtual XElement GetXML()
        {
            XElement Xelement = new XElement("answers");
            list.ForEach(item => Xelement.Add(item.GetXML()));
            return Xelement;
        }
    }
}
