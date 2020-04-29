using System;
using System.Threading.Tasks;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Interfaces;
using MediatR;

namespace EventsApi.Infrastracture.Services
{
    public class MediatorDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public MediatorDomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator ??
                throw new ArgumentException("mediator cannot be null", nameof(mediator));
        }

        public async Task Dispatch(BaseDomainEvent domainEvent)
        {
           await  _mediator.Publish(domainEvent);
        }
    }
}
