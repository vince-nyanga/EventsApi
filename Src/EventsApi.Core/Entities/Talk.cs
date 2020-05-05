using System;
using System.Collections.Generic;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Events;

namespace EventsApi.Core.Entities
{
    public class Talk : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ScheduledDateTime { get; set; }

        public List<Speaker> Speakers { get; set; } = new List<Speaker>();

        public void AddSpeaker(Speaker speaker)
        {
            Speakers.Add(speaker);
            Events.Add(new TalkSpeakerAddedEvent(this, speaker));
        }

        public void RemoveSpeaker(Speaker speaker)
        {
            Speakers.Remove(speaker);
            Events.Add(new TalkSpeakerRemovedEvent(this, speaker));
        }
    }
}
