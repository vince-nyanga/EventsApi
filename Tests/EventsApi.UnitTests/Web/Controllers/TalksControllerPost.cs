using System;
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
    public class TalksControllerPost : BaseControllerTest
    {
        [Test]
        public async Task ShouldReturnCreatedTalk()
        {
            // Arrange
            var sut = fixture.Create<TalksController>();

            mockRepository.Setup(r => r.AddAsync(It.IsAny<Talk>()))
                .Returns(Task.FromResult(fixture.Create<Talk>()));

            // Act
            var response = await sut.Post(fixture.Create<TalkForUpdateDto>());

            // Assert
            response.Result.Should().BeOfType(typeof(CreatedAtActionResult));

            var result = (CreatedAtActionResult)response.Result;
            result.Value.Should().BeOfType(typeof(TalkDto));
            
        } 
    }
}
