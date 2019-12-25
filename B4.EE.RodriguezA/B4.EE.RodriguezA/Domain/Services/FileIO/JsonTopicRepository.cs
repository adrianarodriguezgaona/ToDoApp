using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B4.EE.RodriguezA.Domain.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace B4.EE.RodriguezA.Domain.Services.FileIO
{
    public class JsonTopicRepository : ITopicRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerSettings _serializerSettings;
        public JsonTopicRepository( )
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, "reminderTopicLists.json");
            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public async Task<ReminderTopic> AddReminderTopicList(ReminderTopic topic)
        {
            var topicLists = await GetAllTopiclists();
            topicLists.Add(topic);
            SaveReminderTopicToJsonFile(topicLists);
            return await GetReminderTopicListById(topic.Id);
        }

       

        private async Task <IList<ReminderTopic>> GetAllTopiclists()
        {
            try
            {
                string topicListsJson = File.ReadAllText(_filePath);
                var topicLists = JsonConvert.DeserializeObject<IList<ReminderTopic>>(topicListsJson);
                return await Task.FromResult(topicLists);  //using Task.FromResult to have atleast one await in this async method
            }
            catch  //return empty collection on file not found, deserialization error, ...
            {
                return (new List<ReminderTopic>());
            }
        }

        public async Task<ReminderTopic> DeleteReminderTopicList(Guid id)
        {
            var topicLists = await GetAllTopiclists();
            var topicListToRemove = topicLists.FirstOrDefault(e => e.Id == id);
            topicLists.Remove(topicListToRemove);
            SaveReminderTopicToJsonFile(topicLists);
            return topicListToRemove;
        }

        public async Task<ReminderTopic> GetReminderTopicListById(Guid id)
        {
            var topicLists = await GetAllTopiclists();
            return topicLists.FirstOrDefault(e => e.Id == id);
        }

        public async Task<IQueryable<ReminderTopic>> GetReminderTopicLists()
        {
            var topicLists = await GetAllTopiclists();
            return topicLists.AsQueryable();
        }

        public async Task<ReminderTopic> UpdateReminderTopicList(ReminderTopic topic)
        {
            await DeleteReminderTopicList(topic.Id);
            return await AddReminderTopicList(topic);
        }

        private void SaveReminderTopicToJsonFile(IEnumerable<ReminderTopic> topicLists)
        {
            string topicListsJson = JsonConvert.SerializeObject(topicLists, Formatting.Indented, _serializerSettings);
            File.WriteAllText(_filePath, topicListsJson);
        }
    }
}
