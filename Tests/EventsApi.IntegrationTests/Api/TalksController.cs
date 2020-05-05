using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EventsApi.Web;
using EventsApi.Web.Models.Talks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using Xunit;

namespace EventsApi.IntegrationTests.Api
{
    public class TalksController
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TalksController(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ShouldReturnTalksWithoutSpeakers()
        {
            var response = await _client.GetAsync("/api/talks");

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert
                .DeserializeObject<List<TalkDto>>(stringResponse);

            result.Should().NotBeNullOrEmpty();
            result[0].Speakers.Should().BeNullOrEmpty();
        }


        [Fact]
        public async Task Get_ShouldReturnTalksWithSpeakers()
        {
            var response = await _client.GetAsync("/api/talks?includeSpeakers=true");

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert
                .DeserializeObject<List<TalkDto>>(stringResponse);

            result.Should().NotBeNullOrEmpty();
            result[0].Speakers.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetById_ShouldReturnTalkWithoutSpeakers()
        {
            var response = await _client.GetAsync("/api/talks/1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Should().NotBeNull();
            result.Speakers.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GetById_ShouldReturnTalkWithSpeakers()
        {
            var response = await _client.GetAsync("/api/talks/1?includeSpeakers=true");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Should().NotBeNull();
            result.Speakers.Should().NotBeNullOrEmpty();
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
            var response = await _client.DeleteAsync("/api/talks/2");
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            // verify that talk was deleted
            response = await _client.GetAsync("/api/talks");

            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert
                .DeserializeObject<IEnumerable<TalkDto>>(stringResponse);

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
                Description = SeedData.talk1.Description,
                Title = "Updated Description",
                ScheduledDateTime = SeedData.talk1.ScheduledDateTime
            };
            var jsonString = JsonConvert.SerializeObject(talk);

            var response = await _client
                .PutAsync("/api/talks/1", new StringContent(
                    jsonString, Encoding.UTF8,
                    "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            // Verify
            response = await _client.GetAsync("/api/talks/1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Should().NotBeNull();
            result.Title.Should().Be(talk.Title);

        }

        [Fact]
        public async Task Put_ShouldReturnNotFoundGivenInvalidId()
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
            response.Headers.Location.Should().NotBeNull();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Title.Should().Be(talk.Title);
            result.Description.Should().Be(talk.Description);
            result.ScheduledDateTime.Should().Be(talk.ScheduledDateTime);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestGivenInvalidInput()
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

        [Fact]
        public async Task Patch_ShouldReturnNotFoundGivenInvalidId()
        {
            var patchDoc = new JsonPatchDocument<TalkForUpdateDto>();
            patchDoc.Replace(t => t.Description, "Patched description");

            var jsonString = JsonConvert.SerializeObject(patchDoc);

            var response = await _client.PatchAsync("/api/talks/20", new StringContent(
                jsonString,
                Encoding.UTF8,
                "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Patch_ShouldReturnBadRequestGivenInvalidOperation()
        {
            var patchDoc = new JsonPatchDocument<TalkForUpdateDto>();
            patchDoc.Remove(t => t.Title);

            var jsonString = JsonConvert.SerializeObject(patchDoc);

            var response = await _client.PatchAsync("/api/talks/1", new StringContent(
                jsonString,
                Encoding.UTF8,
                "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Patch_ShouldUpdateTalk()
        {
            var patchDoc = new JsonPatchDocument<TalkForUpdateDto>();
            patchDoc.Replace(t => t.Title, "Patched Title");

            var jsonString = JsonConvert.SerializeObject(patchDoc);

            var response = await _client.PatchAsync("/api/talks/1", new StringContent(
                jsonString,
                Encoding.UTF8,
                "application/json"));

            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            // Verify
            response = await _client.GetAsync("/api/talks/1");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TalkDto>(stringResponse);

            result.Should().NotBeNull();
            result.Title.Should().Be("Patched Title");

        }

    }
}
