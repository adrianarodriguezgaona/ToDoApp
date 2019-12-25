using System;
using System.Collections.Generic;
using System.Text;

namespace B4.EE.RodriguezA.Domain.Models
{
    public class TopicItem
    {
        public Guid Id { get; set; }
        public Guid ReminderTopicId { get; set; }
        public ReminderTopic ParentTopic { get; set; }
        public string ItemName { get; set; }
        public string PhotoSource { get; set; }
        public string MyTopicItem { get; set; }

        public DateTime? ToDoDate { get; set; }
    }
}
