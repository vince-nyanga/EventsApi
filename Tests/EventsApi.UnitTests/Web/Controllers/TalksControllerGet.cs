using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using EventsApi.Core.Entities;
using EventsApi.Web.Controllers;
using EventsApi.Web.Models.Talks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
            
            mockRepository.Setup(r => r.ListAsync<Talk>())
                .Returns(Task.FromResult(fixture.CreateMany<Talk>().ToList()));

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
