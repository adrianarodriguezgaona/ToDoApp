using B4.EE.RodriguezA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace B4.EE.RodriguezA.Tests
{
    class TestData
    {

        public static  ReminderTopic[] TestTopics => new[]
        {
            new ReminderTopic
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "School",

            },

            new ReminderTopic
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Kassa tickets",
            },

             new ReminderTopic
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Tandarts",
            }

        };
    }
}
