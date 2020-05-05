using AutoFixture;
using System.Threading.Tasks;
using NUnit.Framework;
using EventsApi.Web.Controllers;
using EventsApi.Core.Entities;
using Moq;
using EventsApi.Web.Models.Speaker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class SpeakersControllerPut : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnNotFound()
        {
            // Arrange
            var sut = fixture.Create<SpeakersController>();
            mockRepository.Setup(r => r.GetByIdAsync<Speaker>(It.IsAny<int>()))
                .Returns(Task.FromResult<Speaker>(null));

            // Act
            var response = await sut.Put(1, fixture.Create<SpeakerForUpdateDto>());

            // Assert
            response.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResult()
        {
            // Arrange
            var sut = fixture.Create<SpeakersController>();
            mockRepository.Setup(r => r.GetByIdAsync<Speaker>(It.IsAny<int>()))
                .Returns(Task.FromResult(fixture.Create<Speaker>()));

            // Act
            var response = await sut.Put(1, fixture.Create<SpeakerForUpdateDto>());

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
