namespace EventsApi.Core.Entities
{
    // For Many-to-Many relationship
    public class TalkSpeaker
    {
        public int TalkId { get; set; }
        public Talk Talk { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Speaker { get; set; }
    }
}
