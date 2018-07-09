using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NotesApp.IntegrationTests.Extensions;
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
        public async Task Get_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new Note {Body = "Note 1"},
                new Note {Body = "Note 2"}
            };
            var expected = new List<Note>
            {
                new Note {Id = 1, Body = initial[0].Body},
                new Note {Id = 2, Body = initial[1].Body}
            };

            await _client.PostAsync(NotesEndpoint, JsonConvert.SerializeObject(initial[0]).ToStringContent());
            await _client.PostAsync(NotesEndpoint, JsonConvert.SerializeObject(initial[1]).ToStringContent());

            var response = await _client.GetAsync(NotesEndpoint);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNullOrEmpty();

            var actual = JsonConvert.DeserializeObject<Note[]>(responseString);
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_ById_WhenInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"{NotesEndpoint}/1");
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_ById_WhenValidId_ShouldReturnNote()
        {
            var initial = new Note {Body = "Test 1"};
            var serializedInitial = JsonConvert.SerializeObject(initial);
            var expected = new Note {Id = 1, Body = initial.Body};

            var postResponse = await _client.PostAsync(NotesEndpoint, serializedInitial.ToStringContent());
            postResponse.EnsureSuccessStatusCode();

            var uri = postResponse.Headers.Location;

            var getResponse = await _client.GetAsync(uri);
            getResponse.EnsureSuccessStatusCode();

            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            getResponseString.Should().NotBeNullOrWhiteSpace();

            var actual = JsonConvert.DeserializeObject<Note>(getResponseString);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Post_WhenInvalidNote_ShouldReturnBadRequest()
        {
            var actual = await _client.PostAsync(NotesEndpoint, string.Empty.ToStringContent());
            actual.IsSuccessStatusCode.Should().BeFalse();
            actual.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_WhenValidNote_ShouldReturnCreatedAtRoute()
        {
            var initial = new Note {Body = "Note 1"};
            var serializedInitial = JsonConvert.SerializeObject(initial);
            var expected = new Note {Id = 1, Body = initial.Body};

            var response = await _client.PostAsync(NotesEndpoint, serializedInitial.ToStringContent());
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var uri = response.Headers.Location;
            uri.AbsolutePath.Should().EndWith($"{NotesEndpoint}/{expected.Id}");
            
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().NotBeNullOrWhiteSpace();

            var actual = JsonConvert.DeserializeObject<Note>(responseBody);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Delete_WhenInvalidId_ShoulReturnNotFound()
        {
            var response = await _client.DeleteAsync($"{NotesEndpoint}/1");
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_WhenValidId_ShouldReturnOk_AndDeleteNote()
        {
            var initial = new Note {Body = "Note 1"};
            var serializedInitial = JsonConvert.SerializeObject(initial);

            var postResponse = await _client.PostAsync(NotesEndpoint, serializedInitial.ToStringContent());
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