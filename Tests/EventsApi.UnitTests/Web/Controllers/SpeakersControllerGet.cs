using System.Threading.Tasks;
using NUnit.Framework;
using AutoFixture;
using EventsApi.Web.Controllers;
using Moq;
using EventsApi.Core.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class SpeakersControllerGet : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnNotFound()
        {
            // Arrange
            var sut = fixture.Create<SpeakersController>();
            mockRepository.Setup(r => r.GetByIdAsync<Speaker>(It.IsAny<int>()))
                .Returns(Task.FromResult<Speaker>(null));

            // Act
            var response = await sut.Get(1);

            // Assert
            response.Result.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task ShouldReturnSpeakerDto()
        {
            // Arrange
            var sut = fixture.Create<SpeakersController>();
            mockRepository.Setup(r => r.GetByIdAsync<Speaker>(It.IsAny<int>()))
                .Returns(Task.FromResult(fixture.Create<Speaker>()));

            // Act
            var response = await sut.Get(1);

            response.Result.Should().BeOfType(typeof(OkObjectResult));

        }
    }
}
