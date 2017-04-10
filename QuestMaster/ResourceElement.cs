using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace QuestMaster
{
    public struct ResourceTags
    {
        public Dictionary<string, string> tags;
        public List<string> quests;

        public ResourceTags(XElement tags, XElement quests) {
            this.tags = new Dictionary<string, string>();
            this.quests = new List<string>();

            if (tags == null && quests == null) return;

            foreach (XElement tag in tags.Elements())
            {
                this.tags.Add(tag.FirstAttribute.Name.ToString(), tag.FirstAttribute.Value);
            }

            foreach (XElement quest in quests.Elements())
            {
                this.quests.Add(quest.FirstAttribute.Value);
            }

        }

        public XElement getTags() {
            XElement tags = new XElement("tags");
            if (this.quests == null) return tags;
            foreach (KeyValuePair<string, string> tag in this.tags)
            {
                tags.Add(new XElement("tag", new XAttribute(tag.Key, tag.Value)));
            }
            return tags;
        }
        public XElement getQuests()
        {
            XElement quests = new XElement("quests");
            if (this.quests == null) return quests;
            foreach (string quest in this.quests)
            {
                quests.Add(new XElement("quest", new XAttribute("id", quest)));
            }
            return quests;
        }

    }
    class ResourceElement
    {
        public string respath;
        public int id;
        public ResourceTags resourceTags;

        public ResourceElement()
        {
            resourceTags = new ResourceTags();
        }
        public ResourceElement(XElement node)
        {
            respath = node.FirstAttribute.Value;
            id = int.Parse(node.LastAttribute.Value);
            XElement tags;
            XElement quests;
            tags = node.Elements().First();
            quests = node.Elements().Last();
            resourceTags = new ResourceTags(tags, quests);
        }
        
        public XElement  getXml()
        {
            XElement res = new XElement("res", new XAttribute("src", respath), new XAttribute("id", id));

            XElement quests = resourceTags.getQuests();
            XElement tags = resourceTags.getTags();
            
            res.Add(tags);
            res.Add(quests);
            return res;
        }

        public void addTags(string nameTag, string valueTag)
        {
            resourceTags.tags.Add(nameTag, valueTag);
        }
        public void addQuest(string idQuest)
        {
            resourceTags.quests.Add(idQuest);
        }
    }
}
