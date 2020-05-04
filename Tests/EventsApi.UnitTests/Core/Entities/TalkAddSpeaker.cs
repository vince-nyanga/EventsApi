using NUnit.Framework;
using AutoFixture;
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
            var talk = new Talk
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
            talk.AddSpeaker(speaker);

            // Assert
            talk.Speakers.Should().NotBeEmpty();
            talk.Events.Should().NotBeEmpty();
            talk.Events[0].Should().BeOfType(typeof(TalkSpeakerAddedEvent));
            
        }
    }
}
