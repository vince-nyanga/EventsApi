using System.Threading.Tasks;
using AutoFixture;
using EventsApi.Core.Entities;
using EventsApi.Web.Controllers;
using EventsApi.Web.Models.Talks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class TalksControllerPut : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnNotFoundIfTaskIsNull()
        {
            // Arrange
            var sut = fixture.Create<TalksController>();
            mockRepository.Setup(r => r.GetByIdAsync<Talk>(It.IsAny<int>()))
                .Returns(Task.FromResult<Talk>(null));

            // Act
            var response = await sut.Put(1, fixture.Create<TalkForUpdateDto>());

            // Assert
            response.Should().BeOfType(typeof(NotFoundResult));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResult()
        {
            // Arrange
            var sut = fixture.Create<TalksController>();

            mockRepository.Setup(r => r.GetByIdAsync<Talk>(It.IsAny<int>()))
                .Returns(Task.FromResult(fixture.Create<Talk>()));

            mockRepository.Setup(r => r.UpdateAsync<Talk>(It.IsAny<Talk>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await sut.Put(1, fixture.Create<TalkForUpdateDto>());

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
