using B4.EE.RodriguezA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.EE.RodriguezA.Domain.Services
{
    public interface ITopicService
    {
        /// <summary>
        /// Gets all ReminderTopics
        /// </summary>
        Task<IQueryable<ReminderTopic>>GetAll();

        /// <summary>
        /// Gets a ReminderTopic by its Id
        /// </summary>
        Task<ReminderTopic> GetById(Guid topicId);

        /// <summary>
        /// Updates an existing ReminderTopic using its Id, and returns the updated Site on success
        /// </summary>
        /// 
        //Task<ReminderTopic> Update(ReminderTopic topic);

        Task<ReminderTopic> Save(ReminderTopic topic);

        /// <summary>
        /// Deletes a Topic using its Id and returns the removed Topic on success
        /// </summary>
        Task<ReminderTopic> Delete(Guid topicId);


    }
}
