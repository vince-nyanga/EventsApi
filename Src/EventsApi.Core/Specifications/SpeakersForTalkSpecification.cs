using EventsApi.Core.Entities;

namespace EventsApi.Core.Specifications
{
    public class SpeakersForTalkSpecification : BaseSpecification<Speaker>
    {
        public SpeakersForTalkSpecification(int talkId)
            :base(s => s.TalkId == talkId)
        {
        }
    }
}
