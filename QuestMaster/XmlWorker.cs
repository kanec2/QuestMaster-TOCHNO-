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
        public Dictionary<XName, List<ResourceElement>> resources;
        List<ResourceElement> ress;
        public int id;
        public XmlWorker(XElement questMaster, Dictionary<XName, List<ResourceElement>> resources)
        {
            this.resources = resources;
            this.id = int.Parse(questMaster.Elements("resources").First().LastAttribute.Value);
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

        public void writeToXml(XElement doc,int idx)
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
            resource.Add(new XAttribute("id", idx));
            doc.Add(resource);
        }
    }
}
