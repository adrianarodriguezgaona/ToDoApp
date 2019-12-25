using System;
using System.Collections.Generic;
using System.Text;

namespace B4.EE.RodriguezA.Domain.Models
{
    public class ReminderTopic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<TopicItem> Items { get; set; } = new List<TopicItem>();

    }
}
