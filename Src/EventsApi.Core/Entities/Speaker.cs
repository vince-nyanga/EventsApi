using EventsApi.Core.Abstracts;

namespace EventsApi.Core.Entities
{
    public class Speaker : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
