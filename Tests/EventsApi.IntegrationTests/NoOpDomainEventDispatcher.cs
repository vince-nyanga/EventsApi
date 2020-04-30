using System.Threading.Tasks;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Interfaces;

namespace EventsApi.IntegrationTests
{
    public class NoOpDomainEventDispatcher : IDomainEventDispatcher
    { 
        public Task Dispatch(BaseDomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}
