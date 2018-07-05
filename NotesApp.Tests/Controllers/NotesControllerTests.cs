using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApp.Controllers;
using NotesApp.Models;
using NotesApp.Services;
using Xunit;

namespace NotesApp.Tests.Controllers
{
    public class NotesControllerTests
    {
        private readonly Mock<INoteService> _noteServiceMock;
        private readonly NotesController _controller;
        
        public NotesControllerTests()
        {
            _noteServiceMock = new Mock<INoteService>();
            _controller = new NotesController(_noteServiceMock.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldCall_NoteService_GetNotesAsync()
        {
            await _controller.GetAsync();
            _noteServiceMock.Verify(s => s.GetNotesAsync(), Times.Once);
        }
        
        [Fact]
        public async Task GetAsync_ShouldReturn_NoteService_GetNotesAsync()
        {
            var initial = new List<Note>
            {
                new Note { Id = 1, Body = "Note 1" }
            };
            var expected = initial;

            _noteServiceMock
                .Setup(s => s.GetNotesAsync())
                .ReturnsAsync(initial);

            var response = await _controller.GetAsync();
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var actual = response.Result.As<OkObjectResult>().Value?.As<IEnumerable<Note>>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}
