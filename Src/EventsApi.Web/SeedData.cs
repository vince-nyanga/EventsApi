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
        

        public static readonly Talk talk1 = new Talk
        {
            Title = "Domain Driven Design",
            Description = "An introduction to domain driven design",
            ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(2),

            Speakers =
            {
                new Speaker
                {
                    Name = "Test Speaker",
                    Email = "test1@test.com"
                }
            }
            
        };


        public static readonly Talk talk2 = new Talk
        {
            Title = "Developing a web API",
            Description = "An introduction web API",
            ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(3),

            Speakers =
            {
                new Speaker
                {
                    Name = "Awesome Speaker",
                    Email = "test2@test.com"
                }
            }

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
            foreach (var item in dbContext.Talks)
            {
                dbContext.Remove(item);
            }

            dbContext.SaveChanges();

            dbContext.Talks.Add(talk1);
            dbContext.Talks.Add(talk2);

            dbContext.SaveChanges();
        }
    }
}
