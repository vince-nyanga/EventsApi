using NUnit.Framework;
using AutoFixture;
using System.Threading.Tasks;
using EventsApi.Web.Controllers;
using Moq;
using EventsApi.Core.Specifications;
using EventsApi.Core.Entities;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class TalkSpeakersControllerGet : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnListOfSpeakers()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();

            IReadOnlyList<Talk> talks = new List<Talk> { fixture.Create<Talk>() };

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));

            // Act

            var response = await sut.Get(1);

            // Assert

            response.Result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            // Arrange
            var sut = fixture.Create<TalkSpeakersController>();

            IReadOnlyList<Talk> talks = new List<Talk> {  };

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));

            // Act

            var response = await sut.Get(1);

            // Assert

            response.Result.Should().BeOfType(typeof(NotFoundResult));
        }
    }
}
