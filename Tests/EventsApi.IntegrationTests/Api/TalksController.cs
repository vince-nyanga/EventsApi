using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventsApi.Web;
using EventsApi.Web.Models.Talks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace EventsApi.IntegrationTests.Api
{
    public class TalksController : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TalksController(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ShouldReturnTalks()
        {
            var response = await _client.GetAsync("/api/talks");

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<TalkDto>>(stringResponse);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_ShouldReturnTalk()
        {
            var response = await _client.GetAsync("/api/talks/1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Should().NotBeNull();
            result.Title.Should().Be(SeedData.talk1.Title);
            result.Description.Should().Be(SeedData.talk1.Description);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundGivenInvalidId()
        {
            var response = await _client.GetAsync("/api/talks/10");

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Delete_ShouldDeleteTalk()
        {
            var response = await _client.DeleteAsync("/api/talks/1");
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            // verify that talk was deleted
            response = await _client.GetAsync("/api/talks");

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<TalkDto>>(stringResponse);

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFoundGivenInvalidId()
        {
            var response = await _client.DeleteAsync("/api/talks/1000");

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Put_ShouldUpdateTalk()
        {
            var talk = new TalkForUpdateDto
            {
                Description = SeedData.talk2.Description,
                Title = "Updated Description",
                ScheduledDateTime = SeedData.talk2.ScheduledDateTime
            };
            var jsonString = JsonConvert.SerializeObject(talk);

            var response = await _client
                .PutAsync("/api/talks/2", new StringContent(
                    jsonString, Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            // Verify
            response = await _client.GetAsync("/api/talks/2");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Should().NotBeNull();
            result.Title.Should().Be(talk.Title);

        }

        [Fact]
        public async Task Put_ShouldReturnNotFoundForInvalidId()
        {
            var talk = new TalkForUpdateDto
            {
                Description = SeedData.talk1.Description,
                Title = "Updated Description",
                ScheduledDateTime = SeedData.talk1.ScheduledDateTime
            };
            var jsonString = JsonConvert.SerializeObject(talk);

            var response = await _client
                .PutAsync("/api/talks/10", new StringContent(
                    jsonString, Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequestGivenInvalidInput()
        {
            var talk = new TalkForUpdateDto
            {
                Title = "Test talk",
                ScheduledDateTime = SeedData.talk1.ScheduledDateTime
            };
            var jsonString = JsonConvert.SerializeObject(talk);

            var response = await _client
                .PutAsync("/api/talks/1", new StringContent(
                    jsonString, Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Post_ShouldSuccessfullyAddTalk()
        {
            var talk = new TalkForUpdateDto
            {
                Description = "This is a test talk",
                Title = "Test talk",
                ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(2)
            };
            var jsonString = JsonConvert.SerializeObject(talk);

            var response = await _client
                .PostAsync("/api/talks", new StringContent(
                    jsonString, Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(StatusCodes.Status201Created);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Title.Should().Be(talk.Title);
            result.Description.Should().Be(talk.Description);
            result.ScheduledDateTime.Should().Be(talk.ScheduledDateTime);
        }

        [Fact]
        public async Task Post_ShouldUpdateReturnBadRequestIfValidationFails()
        {
            var talk = new TalkForUpdateDto
            {
                Title = "Test talk",
                ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(2)
            };
            var jsonString = JsonConvert.SerializeObject(talk);

            var response = await _client
                .PostAsync("/api/talks", new StringContent(
                    jsonString, Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

    }
}
