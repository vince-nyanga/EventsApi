using EventsApi.Core.Abstracts;
using EventsApi.Core.Entities;

namespace EventsApi.Core.Events
{
    public class NewTalkAddedEvent : BaseDomainEvent
    {
        public NewTalkAddedEvent(Talk talk)
        {
            Talk = talk;
        }

        public Talk Talk { get; }
    }
}
