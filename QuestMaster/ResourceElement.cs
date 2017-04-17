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
        public List<string> tags;

        public ResourceTags(XElement tags) {
            this.tags = new List<string>();

            if (tags == null) return;

            foreach (XElement tag in tags.Elements())
            {
                this.tags.Add(tag.Value);
            }

        }

        public XElement getTags() {
            XElement tags = new XElement("tags");
           
            foreach (string tag in this.tags)
            {
                tags.Add(new XElement("tag", new XText(tag)));
            }
            return tags;
        }
        

    }
    public class ResourceElement
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

            tags = node.Elements().First();
 
            resourceTags = new ResourceTags(tags);
        }
        
        public XElement  getXml()
        {
            XElement res = new XElement("res", new XAttribute("src", respath), new XAttribute("id", id));

            XElement tags = resourceTags.getTags();
            
            res.Add(tags);

            return res;
        }

        public void addTags(string nameTag)
        {
            resourceTags.tags.Add(nameTag);
        }

    }
}
