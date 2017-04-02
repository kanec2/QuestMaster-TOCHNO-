using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace QuestMaster
{
    class ResourceElement
    {
        public Dictionary<string, string> tags;
        public List<string> quests;
        public string respath;
        public int id;

        public ResourceElement()
        {
            this.tags = new Dictionary<string, string>();
            this.quests = new List<string>();
        }
        public ResourceElement(XElement node)
        {
            this.tags = new Dictionary<string, string>();
            this.quests = new List<string>();

            respath = node.FirstAttribute.Value;
            id = int.Parse(node.LastAttribute.Value);

            XElement tags = node.Elements().First();
            XElement quests = node.Elements().Last();

            foreach (XElement tag in tags.Elements())
            {
                this.tags.Add(tag.FirstAttribute.Name.ToString(), tag.FirstAttribute.Value);
            }

            foreach (XElement quest in quests.Elements())
            {
                this.quests.Add(quest.FirstAttribute.Value);
            }
        }
        
        public XElement  getXml()
        {
            XElement res = new XElement("res", new XAttribute("src", respath), new XAttribute("id", id));
            XElement tags = new XElement("tags");
            XElement quests = new XElement("quests");

            foreach (KeyValuePair<string, string> tag in this.tags)
            {
                tags.Add(new XElement("tag", new XAttribute(tag.Key, tag.Value)));
            }
            foreach (string quest in this.quests)
            {
                quests.Add(new XElement("quest", new XAttribute("id", quest)));
            }

            res.Add(tags);
            res.Add(quests);
            return res;
        }

        public void addTags(string nameTag, string valueTag)
        {
            tags.Add(nameTag, valueTag);
        }
        public void addQuest(string idQuest)
        {
            quests.Add(idQuest);
        }
    }
}
