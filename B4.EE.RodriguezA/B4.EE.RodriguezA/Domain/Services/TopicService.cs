using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Validators;
using FluentValidation;

namespace B4.EE.RodriguezA.Domain.Services
{
    public class TopicService : ITopicService

    {
        private ITopicRepository _topicRepository;
        private IValidator _topicValidator;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            _topicValidator = new TopicValidator();
        }

        public async Task<ReminderTopic> Delete(Guid topicId)
        {
            return await _topicRepository.DeleteReminderTopicList(topicId);
        }

        public async Task<IQueryable<ReminderTopic>> GetAll()
        {
            var reminderTopicLists = await _topicRepository.GetReminderTopicLists();
            return reminderTopicLists.AsQueryable();
        }

        public async Task<ReminderTopic> GetById(Guid topicId)
        {
            return await _topicRepository.GetReminderTopicListById(topicId);
        }

        public Task<ReminderTopic> Save(ReminderTopic topic)
        {
            
            var results = _topicValidator.Validate(topic);
            var errors = results.Errors;
            if (results.IsValid)
            {
                var existingTopic = _topicRepository.GetReminderTopicListById(topic.Id);
                if (existingTopic != null)
                {
                    var savedTopic = _topicRepository.UpdateReminderTopicList(topic);
                    return savedTopic;
                }
                else
                {
                    var savedTopic = _topicRepository.AddReminderTopicList(topic);
                    return savedTopic;
                }
            }
            else
            {
                throw new ValidationException(results.Errors);
            }
        }

       
    }
}
