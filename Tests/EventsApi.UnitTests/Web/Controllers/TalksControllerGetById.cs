using System.Threading.Tasks;
using NUnit.Framework;
using AutoFixture;
using EventsApi.Core.Entities;
using Moq;
using EventsApi.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using EventsApi.Web.Models.Talks;
using EventsApi.Core.Specifications;
using System.Collections.Generic;

namespace EventsApi.UnitTests.Web.Controllers
{
    
    public class TalksControllerGetById : BaseTalksControllerTest
    {

        [Test]
        public async Task  ShouldReturnTalk()
        {
            // Arrange
            var sut = fixture.Create<TalksController>();
            IReadOnlyList<Talk> talks = new List<Talk> { fixture.Create<Talk>() };

            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalkWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));

            // Act
            var response = await sut.Get(1);

            // Assert
            response.Result.Should().BeOfType(typeof(OkObjectResult));

            var result = (OkObjectResult)response.Result;

            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType(typeof(TalkDto));
        }

        [Test]
        public async Task ShouldReturnNotFound()
        {
            // Arrange
            var sut = fixture.Create<TalksController>();
            mockRepository.Setup(r => r.GetByIdAsync<Talk>(It.IsAny<int>()))
                .Returns(Task.FromResult<Talk>(null));

            // Act
            var response = await sut.Get(1);

            // Assert
            response.Result.Should().BeOfType(typeof(NotFoundResult));
        }
    }
}
