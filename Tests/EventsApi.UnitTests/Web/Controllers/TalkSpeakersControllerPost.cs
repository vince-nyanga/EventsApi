using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using EventsApi.Core.Entities;
using EventsApi.Core.Specifications;
using EventsApi.Web.Controllers;
using EventsApi.Web.Models.Speaker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class TalkSpeakersControllerPost : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnNotFound()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();
            IReadOnlyList<Talk> talks = new List<Talk> { };

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));
            // Act
            var response = await sut.Post(1, fixture.Create<SpeakerForUpdateDto>());

            // Assert
            response.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResult()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();
            IReadOnlyList<Talk> talks = new List<Talk> { fixture.Create<Talk>()};

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));
            // Act
            var response = await sut.Post(1, fixture.Create<SpeakerForUpdateDto>());

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
