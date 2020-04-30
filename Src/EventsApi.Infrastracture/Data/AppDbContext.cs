using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventsApi.Core.Abstracts;
using EventsApi.Core.Entities;
using EventsApi.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventsApi.Infrastracture.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IDomainEventDispatcher domainEventDispatcher)
            :base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public DbSet<Talk> Talks { get; set; }
        public DbSet<Speaker> Speakers { get; set; }

      

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


            if (_domainEventDispatcher == null)
            {
                return result;
            }


            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach (var domainEvent in events)
                {
                    await _domainEventDispatcher.Dispatch(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
