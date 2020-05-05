using NUnit.Framework;
using FluentAssertions;
using EventsApi.Core.Entities;
using System;
using EventsApi.Core.Events;

namespace EventsApi.UnitTests.Core.Entities
{
    [TestFixture]
    public class TalkAddSpeaker
    {
        [Test]
        public void ShouldRaiseTalkSpeakerAddedEvent()
        {
            // Arrange
            var sut = new Talk
            {
                Description = "my talk",
                Title = "my talk",
                ScheduledDateTime = DateTimeOffset.UtcNow,
            };

            var speaker = new Speaker
            {
                Name = "Awesome Speaker",
                Email = "email@email.com"
            };
            // Act
            sut.AddSpeaker(speaker);

            // Assert
            sut.Speakers.Should().NotBeEmpty();
            sut.Events.Should().NotBeEmpty();
            sut.Events[0].Should().BeOfType(typeof(TalkSpeakerAddedEvent));
            
        }
    }
}
