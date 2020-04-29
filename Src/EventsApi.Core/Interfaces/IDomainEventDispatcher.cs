using System.Threading.Tasks;
using EventsApi.Core.Abstracts;

namespace EventsApi.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(BaseDomainEvent domainEvent);
    }
}
