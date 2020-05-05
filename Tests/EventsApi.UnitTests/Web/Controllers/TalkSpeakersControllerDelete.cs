using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using EventsApi.Core.Entities;
using EventsApi.Core.Specifications;
using EventsApi.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class TalkSpeakersControllerDelete : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnNotFound_If_Talk_Does_Not_Exist()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();
            IReadOnlyList<Talk> talks = new List<Talk> { };

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));
            // Act
            var response = await sut.Delete(1, 1);

            // Assert
            response.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task ShouldReturnNotFound_If_Speaker_Does_Not_Exist()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();
            var talk = fixture.Create<Talk>();
            talk.Speakers = new List<Speaker>();
            IReadOnlyList<Talk> talks = new List<Talk> { talk };

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));
            // Act
            var response = await sut.Delete(1, 1);

            // Assert
            response.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResult()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();

            var talk = fixture.Create<Talk>();

            IReadOnlyList<Talk> talks = new List<Talk> {talk};

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));
            // Act
            var response = await sut.Delete(1, talk.Speakers[0].Id);

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
