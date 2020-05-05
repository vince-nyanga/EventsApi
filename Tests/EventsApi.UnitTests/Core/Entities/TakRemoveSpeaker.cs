using System;
using EventsApi.Core.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace EventsApi.UnitTests.Core.Entities
{
    [TestFixture]
    public class TakRemoveSpeaker
    {
        [Test]
        public void ShouldRaiseTalkSpeakerRemovedEvent()
        {
            // Arrange
            var sut = new Talk
            {
                Description = "my talk",
                Title = "my talk",
                ScheduledDateTime = DateTimeOffset.UtcNow,
                Speakers =
                {
                     new Speaker
                    {
                        Name = "Awesome Speaker",
                        Email = "email@email.com"
                    }
                }
            };

            // Act
            sut.RemoveSpeaker(sut.Speakers[0]);

            // Assert
            sut.Speakers.Should().BeEmpty();
            sut.Events.Should().NotBeEmpty();
        }
    }
}
