using NUnit.Framework;
using EventsApi.Web.Controllers;
using EventsApi.Core.Entities;
using System.Threading.Tasks;
using Moq;
using AutoFixture;
using Microsoft.AspNetCore.JsonPatch;
using EventsApi.Web.Models.Talks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class TalksControllerPatch : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnNotFound()
        {
            // Arrange
            var sut = fixture.Create<TalksController>();

            mockRepository.Setup(r => r.GetByIdAsync<Talk>(It.IsAny<int>()))
                .Returns(Task.FromResult<Talk>(null));

            var patchDoc = new JsonPatchDocument<TalkForUpdateDto>();
            patchDoc.Replace(t => t.Description, "New description");

            // Act
            var response = await sut.Patch(1, patchDoc);

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

            var patchDoc = new JsonPatchDocument<TalkForUpdateDto>();
            patchDoc.Replace(t => t.Description, "New description");

            // Act
            var response = await sut.Patch(1, patchDoc);

            // Assert
            response.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
