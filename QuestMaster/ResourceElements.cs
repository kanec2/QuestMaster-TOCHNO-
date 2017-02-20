using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace QuestMaster
{
    class ResourceElements
    {
        Dictionary<string, string> tags;
        List<string> quests;
        string respath;

        ResourseElements(XElement node)
        {
            this.tags = new Dictionary<string, string>();
            this.quests = new List<string>();

            respath = node.FirstAttribute.Value;

            XElement tags = node.Elements().First();
            XElement quests = node.Elements().Last();

            foreach (KeyValuePair<string,string> tag in this.tags)
            {
                tag.Key = tags.FirstAttribute.Name;
                tag.Value = tags.FirstAttribute.Value;
            }
        }
        
    }
}
