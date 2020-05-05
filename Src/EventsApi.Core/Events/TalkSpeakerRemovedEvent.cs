using EventsApi.Core.Abstracts;
using EventsApi.Core.Entities;

namespace EventsApi.Core.Events
{
    public class TalkSpeakerRemovedEvent : BaseDomainEvent
    {
        public TalkSpeakerRemovedEvent(Talk talk, Speaker speaker)
        {
            Talk = talk;
            Speaker = speaker;
        }

        public Talk Talk { get; }
        public Speaker Speaker { get; }
    }
}
