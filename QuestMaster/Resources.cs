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
        /// <param name="questsId">ID квеста.</param>
        /// <param name="tagsKey">Имя тега.</param>
        /// <param name="tagsValue">Значение тега.</param>
        public void Add(string nameXName, string src, string questsId, string tagsKey,string tagsValue)
        {
            resourceTags.quests.Add(questsId);
            resourceTags.tags.Add(tagsKey, tagsValue);
            resources[nameXName].Add(new ResourceElement() {respath = src, resourceTags = this.resourceTags, id = this.id});
            this.id++;
        }

        /// <summary>
        /// Добавление нового ресурса, к нему Тега и Квеста.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла.</param>
        /// <param name="questsId">ID квеста.</param>
        public void Add(string nameXName, string src , string questsId)
        {
            resourceTags.quests.Add(questsId);
            resources[nameXName].Add(new ResourceElement() { respath = src, resourceTags = this.resourceTags,  id = this.id });
            this.id++;
        }

        /// <summary>
        /// Добавление нового ресурса, к нему Тега и Квеста.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла.</param>
        /// <param name="tagsKey">Имя Тега.</param>
        /// <param name="tagsValue">Значение Тега.</param>
        public void Add(string nameXName, string src, string tagsKey, string tagsValue)
        {
            resourceTags.tags.Add(tagsKey, tagsValue);
            resources[nameXName].Add(new ResourceElement() { respath = src, resourceTags = this.resourceTags, id = this.id });
            this.id++;
        }

        
        internal ResourceElement checkElement(string name)
        {
            return resources.Select(t => t.Value.Where(v => v.respath == name).First()).First();
        }

        /// <summary>
        /// Удаляет элемент.
        /// </summary>
        /// <param name="id">ID Ресурса.</param>
        public void delete(int id)
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
        public void insert(int id, string state, string key, string value)
        {
            switch (state)
            {
                case "tags":
                    foreach (KeyValuePair<XName, List<ResourceElement>> resource in resources)
                    {
                        foreach (ResourceElement resElem in resource.Value)
                        {
                            if (resElem.id == id)
                            {
                                resElem.resourceTags.tags[key] = value;
                            }
                        }
                    }
                    break;
                case "quests":
                    foreach (KeyValuePair<XName, List<ResourceElement>> resource in resources)
                    {
                        foreach (ResourceElement resElem in resource.Value)
                        {
                            if (resElem.id == id)
                            {
                                resElem.resourceTags.quests[resElem.resourceTags.quests.FindIndex(quest => quest == key)] = value;
                            }
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Добавление id квеста.
        /// </summary>
        /// <param name="id">ID ресурса.</param>
        /// <param name="newId">ID Квеста.</param>
        public void update(int id, string questId)
        {
            this.resElem = this.resources.Select(resource => resource.Value.Where(res => res.id == id).First()).First();
            this.resElem.addQuest(questId);
        }

        /// <summary>
        /// Добавление нового тега и его значение.
        /// </summary>
        /// <param name="id">ID Ресурса.</param>
        /// <param name="tagName">Имя тега.</param>
        /// <param name="tagValue">Значение тега.</param>
        public void update(int id, string tagName, string tagValue)
        {
            this.resElem = this.resources.Select(resource => resource.Value.Where(res => res.id == id).First()).First();
            this.resElem.addTags(tagName, tagValue);
        }

        public void update(int id, string questId, string tagName, string tagValue)
        {
            this.resElem = this.resources.Select(resource => resource.Value.Where(res => res.id == id).First()).First();
            this.resElem.addQuest(questId);
            this.resElem.addTags(tagName, tagValue);
        }
        
        public void save()
        {
            xWork.writeToXml(doc.Elements("QuestMaster").First(),id);
            doc.Save("Resources//XMLMap.xml");
        }
    }
}
