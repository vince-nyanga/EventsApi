using EventsApi.Core.Abstracts;
using EventsApi.Core.Entities;

namespace EventsApi.Core.Events
{
    public class TalkDeletedEvent : BaseDomainEvent
    {
        public TalkDeletedEvent(Talk talk)
        {
            Talk = talk;
        }

        public Talk Talk { get; }
    }
}
