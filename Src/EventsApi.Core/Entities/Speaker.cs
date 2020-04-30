using System.Collections.Generic;
using EventsApi.Core.Abstracts;

namespace EventsApi.Core.Entities
{
    public class Speaker : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public ICollection<Talk> Talks { get; set; } = new List<Talk>();
    }
}
