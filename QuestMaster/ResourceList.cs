using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuestMaster
{
    class ResourceList
    {
        Dictionary<XName, List<ResourceElement>> resources = new Dictionary<XName, List<ResourceElement>>();
        List<ResourceElement> resList;
        ResourceElement resElem;
        List<string> quests = new List<string>();
        Dictionary<string, string> tags = new Dictionary<string, string>();
        XmlWorker xWork;
        int id;

        public ResourceList()
        {
            xWork = new XmlWorker(resources);
            this.id = xWork.finId;
        }

        /// <summary>
        /// Добавление нового ресурса.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла</param>
        public void add(string nameXName, string src)
        {
            this.resources[nameXName].Add(new ResourceElement() { respath = src, id = this.id});
            this.id++;
        }

        /// <summary>
        /// Добавление нового ресурса, к нему Тега и квеста.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла.</param>
        /// <param name="questsId">ID квеста.</param>
        /// <param name="tagsKey">Имя тега.</param>
        /// <param name="tagsValue">Значение тега.</param>
        public void add(string nameXName, string src, string questsId, string tagsKey,string tagsValue)
        {
            quests.Add(questsId);
            tags.Add(tagsKey, tagsValue);
            resources[nameXName].Add(new ResourceElement() {respath = src, quests = quests, tags = tags, id = this.id});
            this.id++;
        }

        /// <summary>
        /// Добавление нового ресурса, к нему Тега и квеста.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла.</param>
        /// <param name="questsId">ID квеста.</param>
        public void add(string nameXName, string src , string questsId)
        {
            quests.Add(questsId);
            resources[nameXName].Add(new ResourceElement() { respath = src, quests = quests,  id = this.id });
            this.id++;
        }

        /// <summary>
        /// Добавление нового ресурса, к нему Тега и квеста.
        /// </summary>
        /// <param name="nameXName">Images,Videos,Audios,Text</param>
        /// <param name="src">Путь до файла.</param>
        /// <param name="tagsKey">Имя Тега.</param>
        /// <param name="tagsValue">Значение Тега.</param>
        public void add(string nameXName, string src, string tagsKey, string tagsValue)
        {
            tags.Add(tagsKey, tagsValue);
            resources[nameXName].Add(new ResourceElement() { respath = src, tags = tags, id = this.id });
            this.id++;
        }

        /// <summary>
        /// Удаляет элемент.
        /// </summary>
        /// <param name="id">ID Ресурса.</param>
        public void delete(int id)
        {
            resources.Select(resource => resource.Value.RemoveAll(elem => elem.id == id));

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
                case "tags": resources.Select(resource => resource.Value.Where(res => res.id == id).First().tags.Select(tag => tag.Key == key ? tag.Value.Replace(tag.Value.ToString(), value) : null)); break;
                case "quests": resources.Select(resource => resource.Value.Where(res => res.id == id).First().quests.Select(quest => quest == key? quest.Replace(quest.ToString(), value): null)); break;
            }
           
        }

        /// <summary>
        /// Добавление id квеста.
        /// </summary>
        /// <param name="id">ID ресурса.</param>
        /// <param name="newId">ID Квеста.</param>
        public void update(int id, string questId)
        {
            resElem = resources.Select(resource => resource.Value.Where(res => res.id == id).First()).First();
            resElem.addQuest(questId);
        }

        /// <summary>
        /// Добавление нового тега и его значение.
        /// </summary>
        /// <param name="id">ID Ресурса.</param>
        /// <param name="tagName">Имя тега.</param>
        /// <param name="tagValue">Значение тега.</param>
        public void update(int id, string tagName, string tagValue)
        {
            resElem = resources.Select(resource => resource.Value.Where(res => res.id == id).First()).First();
            resElem.addTags(tagName, tagValue);
        }
    }
}
