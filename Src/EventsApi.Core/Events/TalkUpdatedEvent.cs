using EventsApi.Core.Abstracts;
using EventsApi.Core.Entities;

namespace EventsApi.Core.Events
{
    public class TalkUpdatedEvent : BaseDomainEvent
    {
        public TalkUpdatedEvent(Talk talk)
        {
            Talk = talk;
        }

        public Talk Talk { get; }
    }
}
