using System;
using MediatR;
namespace EventsApi.Core.Abstracts
{
    public abstract class BaseDomainEvent : INotification
    {
        public DateTimeOffset DateTimeOccured { get; protected set; } =
            DateTimeOffset.UtcNow;
    }
}
