using System;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Events;

namespace EventsApi.Core.Entities
{
    public class Talk : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ScheduledDateTime { get; set; }

        public Speaker Speaker { get; set; }

        public void AddSpeaker(Speaker speaker)
        {
            Speaker = speaker;
            Events.Add(new TalkSpeakerAddedEvent(this, speaker));
        }
    }
}
