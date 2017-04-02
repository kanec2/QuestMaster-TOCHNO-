using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace QuestMaster
{
    class XmlWorker
    {
        public Dictionary<XName, List<ResourceElement>> resources = new Dictionary<XName, List<ResourceElement>>();
        List<ResourceElement> ress;
        public int id = 1;
        public int finId;
        public XmlWorker(Dictionary<XName, List<ResourceElement>> resources)
        {
            resources = this.resources;
        }
        public XmlWorker(XElement questMaster)
        {
            this.finId = int.Parse(questMaster.Elements("resources").First().LastAttribute.Value);
            foreach (XElement resource in questMaster.Elements("resources").Elements())
            {
                ress = new List<ResourceElement>();
                foreach (XElement res in resource.Elements())
                {
                    ress.Add(new ResourceElement(res));
                }
                this.resources.Add(resource.Name, ress);
            }
        }

        public void writeToXml(XElement doc)
        {
           
            XElement resource = new XElement("resources");
            
            foreach(KeyValuePair<XName, List<ResourceElement>> res in this.resources)
            {
                XElement name = new XElement(res.Key);

                foreach (ResourceElement item in res.Value)
                {
                   XElement el =  item.getXml();
                    name.Add(el);
                    if (this.id < item.id)
                    {
                        this.id = item.id;
                    }   
                }
                resource.Add(name);
            }
            doc.RemoveAll();
            id++;
            resource.Add(new XAttribute("id", id));
            doc.Add(resource);
        }
    }
}
