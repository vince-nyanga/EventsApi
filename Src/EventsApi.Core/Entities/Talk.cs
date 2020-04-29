using System;
using EventsApi.Core.Abstracts;

namespace EventsApi.Core.Entities
{
    public class Talk : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ScheduledDateTime { get; set; }
        public Speaker Speaker { get; set; }
    }
}
