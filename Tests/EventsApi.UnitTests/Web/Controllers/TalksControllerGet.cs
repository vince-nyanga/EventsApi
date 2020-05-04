using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EventsApi.Core.Entities;
using EventsApi.Core.Specifications;
using EventsApi.Web.Controllers;
using EventsApi.Web.Models.Talks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EventsApi.UnitTests.Web.Controllers
{
    public class TalksControllerGet : BaseTalksControllerTest
    {
        [Test]
        public async Task ShouldReturnListOfTalks()
        {

            // Arrange
            var sut = fixture.Create<TalksController>();

            IReadOnlyList<Talk> talks = fixture.CreateMany<Talk>().ToList();
            mockRepository.Setup(r => r.ListAsync(It.IsAny<TalksWithSpeakersSpecification>()))
                .Returns(Task.FromResult(talks));

            // Act
            var response = await sut.Get();

            // Assert
            response.Result.Should().BeOfType(typeof(OkObjectResult));

            var result = (OkObjectResult)response.Result;

            result.Value.Should().NotBeNull();
            ((List<TalkDto>)result.Value).Should().NotBeEmpty();

        }
    }
}
