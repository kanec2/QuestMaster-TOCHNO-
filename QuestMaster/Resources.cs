using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QuestMaster
{
    class Resources
    {
        public Dictionary<XName, List<ResourceElement>> resources;
        ResourceElement resElem;
        List<string> quests = new List<string>();
        Dictionary<string, string> tags = new Dictionary<string, string>();
        ResourceTags resourceTags = new ResourceTags();
        XmlWorker xWork;
        int id;
        XDocument doc = XDocument.Load("Resources//XMLMap.xml");

        public Resources()
        {
            resources = new Dictionary<XName, List<ResourceElement>>();
            xWork = new XmlWorker(doc.Elements().Last(), resources);
            this.id = xWork.id;
        }



        /// <summary>
        /// Добавление нового ресурса.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла</param>
        public void Add(string nameXName, string src)
        {
            this.resources[nameXName].Add(new ResourceElement() { respath = src, id = this.id });
            this.id++;
        }

        /// <summary>
        /// Добавление нового ресурса, к нему Тега и Квеста.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла.</param>
        /// <param name="tagsName">Имя тега.</param>
        public void Add(string nameXName, string src, string tagName)
        {
            resourceTags.tags.Add(tagName);
            resources[nameXName].Add(new ResourceElement() {respath = src, resourceTags = this.resourceTags, id = this.id});
            this.id++;
        }

        /// <summary>
        /// Получаем ресурс Элемент.
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <returns></returns>
        internal ResourceElement checkElement(string name)
        {
            ResourceElement elem = null;
            foreach (KeyValuePair<XName, List<ResourceElement>> item in this.resources)
            {
                foreach (ResourceElement res in item.Value)
                {
                    if (res.respath == name)
                    {
                        elem = res;
                    }
                }
            }
            return elem;
        }

        /// <summary>
        /// Удаляет элемент.
        /// </summary>
        /// <param name="id">ID Ресурса.</param>
        public void Delete(int id)
        {
            foreach (KeyValuePair<XName, List<ResourceElement>> resource in resources)
            {
                foreach (ResourceElement resElem in resource.Value)
                {
                    if (resElem.id == id)
                    {
                        resource.Value.Remove(resElem);
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// Изменяет значение тегов или квестов.
        /// </summary>
        /// <param name="id">ID ресурса.</param>
        /// <param name="state">tags или quests.</param>
        /// <param name="key">Для Тега: имя тега. Для Квеста: старое значение тега.</param>
        /// <param name="value">Для Тега: новое значение тега. Для Квеста: новый Квест.</param>
        public void Insert(int id, string value)
        {
            foreach (KeyValuePair<XName, List<ResourceElement>> resource in resources)
            {
                foreach (ResourceElement resElem in resource.Value)
                {
                    if (resElem.id == id && !resElem.resourceTags.tags.Contains(value))
                    {
                        resElem.resourceTags.tags.Add(value);
                    }
                }
            }

        }

        /// <summary>
        /// Добавление id квеста.
        /// </summary>
        /// <param name="id">ID Ресурса.</param>
        /// <param name="tagName">Имя тега.</param>
        public void Update(int id, string tagName)
        {
            this.resElem = this.resources.Select(resource => resource.Value.Where(res => res.id == id).First()).First();
            this.resElem.addTags(tagName);
        }
        
        /// <summary>
        /// Сохраняем файл с ресурсами.
        /// </summary>
        public void Save()
        {
            xWork.writeToXml(doc.Elements("QuestMaster").First(),id);
            doc.Save("Resources//XMLMap.xml");
        }

        /// <summary>
        /// Производим переиндексацию ресурсов.
        /// </summary>
        public void ReIndex()
        {
            int id = 1;
            foreach (KeyValuePair<XName,List<ResourceElement>> item in this.resources)
            {
                foreach (ResourceElement resElem in item.Value)
                {
                    resElem.id = id;
                    id++;
                }
            }
            this.id = id;
        }
        public List<string> getAllTags() {
            List<string> allTags = new List<string>();
            foreach (KeyValuePair<XName,List<ResourceElement>> res in resources)
            {
                foreach (ResourceElement resElem in res.Value)
                {
                    allTags.AddRange(resElem.resourceTags.tags);
                    allTags = allTags.Distinct().ToList();
                }
            }
            return allTags;
        }
        public List<ResourceElement> findByTag(string tag) {
            return null;
        }
    }
}
