using EventsApi.Core.Entities;

namespace EventsApi.Core.Specifications
{
    public class TalkWithSpeakersSpecification : BaseSpecification<Talk>
    {
        public TalkWithSpeakersSpecification(int id)
            :base(t => t.Id == id)
        {
            AddInclude(t => t.Speakers);
        }
    }
}
