using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NotesApp.Models;
using Xunit;
using static NotesApp.IntegrationTests.Utils.HttpClientUtils;

namespace NotesApp.IntegrationTests.Controllers
{
    public class NotesControllerTests
    {
        private const string NotesEndpoint = "/api/notes";
        private readonly HttpClient _client;

        public NotesControllerTests()
        {
            _client = CreateClient();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new Note { Body = "Note 1" },
                new Note { Body = "Note 2" }
            };
            var expected = new List<Note>
            {
                new Note { Id = 1, Body = initial[0].Body },
                new Note { Id = 2, Body = initial[1].Body }
            };

            await _client.PostAsJsonAsync(NotesEndpoint, initial[0]);
            await _client.PostAsJsonAsync(NotesEndpoint, initial[1]);

            var response = await _client.GetAsync(NotesEndpoint);
            response.EnsureSuccessStatusCode();

            var actual = await response.Content.ReadAsAsync<Note[]>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAsync_ById_WhenInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"{NotesEndpoint}/1");
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAsync_ById_WhenValidId_ShouldReturnNote()
        {
            var initial = new Note { Body = "Test 1" };
            var expected = new Note { Id = 1, Body = initial.Body };

            var postResponse = await _client.PostAsJsonAsync(NotesEndpoint, initial);
            postResponse.EnsureSuccessStatusCode();

            var uri = postResponse.Headers.Location;

            var getResponse = await _client.GetAsync(uri);
            getResponse.EnsureSuccessStatusCode();

            var actual = await getResponse.Content.ReadAsAsync<Note>();
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostAsync_WhenInvalidNote_ShouldReturnBadRequest()
        {
            var actual = await _client.PostAsJsonAsync<Note>(NotesEndpoint, null);
            actual.IsSuccessStatusCode.Should().BeFalse();
            actual.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostAsync_WhenValidNote_ShouldReturnCreatedAtRoute()
        {
            var initial = new Note { Body = "Note 1" };
            var expected = new Note { Id = 1, Body = initial.Body };

            var response = await _client.PostAsJsonAsync(NotesEndpoint, initial);
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var uri = response.Headers.Location;
            uri.AbsolutePath.Should().EndWith($"{NotesEndpoint}/{expected.Id}");
            
            var actual = await response.Content.ReadAsAsync<Note>();
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteAsync_WhenInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.DeleteAsync($"{NotesEndpoint}/1");
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteAsync_WhenValidId_ShouldReturnOk_AndDeleteNote()
        {
            var initial = new Note { Body = "Note 1" };

            var postResponse = await _client.PostAsJsonAsync(NotesEndpoint, initial);
            postResponse.EnsureSuccessStatusCode();

            var uri = postResponse.Headers.Location;

            var deleteResponse = await _client.DeleteAsync(uri);
            deleteResponse.EnsureSuccessStatusCode();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _client.GetAsync(uri);
            getResponse.IsSuccessStatusCode.Should().BeFalse();
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
