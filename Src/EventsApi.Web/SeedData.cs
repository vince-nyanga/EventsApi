using System;
using System.Linq;
using EventsApi.Core.Entities;
using EventsApi.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventsApi.Web
{
    public static class SeedData
    {
        public static readonly Speaker Speaker1 = new Speaker
        {
            FirstName = "Vincent",
            LastName = "Nyanga",
            Email = "test@test.com",
            Bio = "Software developer"
        };

        public static readonly Talk talk1 = new Talk
        {
            Title = "Domain Driven Design",
            Description = "An introduction to domain driven design",
            ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(2),
            Speaker = Speaker1
        };


        public static readonly Talk talk2 = new Talk
        {
            Title = "Developing a web API",
            Description = "An introduction web API",
            ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(3),
            Speaker = Speaker1
        };


        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new AppDbContext(
               serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null))
            {
                
             
                if (dbContext.Talks.Any())
                {
                    return;
                }

                PopulateTestData(dbContext);


            }
        }

        private static void PopulateTestData(AppDbContext dbContext)
        {
            dbContext.Speakers.Add(Speaker1);
            dbContext.Talks.Add(talk1);
            dbContext.Talks.Add(talk2);
            dbContext.SaveChanges();
        }
    }
}
