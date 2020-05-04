using System;
using System.Collections.Generic;
using EventsApi.Web.Models.Speaker;

namespace EventsApi.Web.Models.Talks
{
    public class TalkDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ScheduledDateTime { get; set; }
        public List<SpeakerDto> Speakers { get; set; }
    }
}
