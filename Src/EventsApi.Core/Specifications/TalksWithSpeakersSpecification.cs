using EventsApi.Core.Entities;

namespace EventsApi.Core.Specifications
{
    public class TalksWithSpeakersSpecification : BaseSpecification<Talk>
    {
        public TalksWithSpeakersSpecification()
        {
            AddInclude(t => t.Speakers);
        }
    }
}
