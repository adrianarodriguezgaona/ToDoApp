using B4.EE.RodriguezA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.EE.RodriguezA.Domain.Services
{
    public interface ITopicRepository
    {
        Task<ReminderTopic> GetReminderTopicListById(Guid id);


        /// <summary>
        /// Updates an existing ReminderTopic instance in the data store
        /// </summary>
        /// <param name="topic">The ReminderTopic instance to persist in the data store</param>
        /// <returns>The updated ReminderTopic instance on success</returns>
        Task<ReminderTopic> UpdateReminderTopicList(ReminderTopic topic);

        /// <summary>
        /// Adds a ReminderTopic instance to the data store
        /// </summary>
        /// <param name="topic">The ReminderTopic instance to persist in the data store</param>
        /// <returns>The added ReminderTopic instance on success</returns>
        Task<ReminderTopic> AddReminderTopicList(ReminderTopic topic);

        Task<IQueryable<ReminderTopic>> GetReminderTopicLists();

        /// <summary>
        /// Deletes an existing ReminderTopic instance from the data store
        /// </summary>
        /// <param name="id">The id of a ReminderTopic instance</param>
        /// <returns>The deleted ReminderTopic instance</returns>
        Task<ReminderTopic> DeleteReminderTopicList(Guid id);
    }

}
