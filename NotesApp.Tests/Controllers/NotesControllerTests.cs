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

        [Fact]
        public async Task GetAsync_ById_ShouldCall_NoteService_GetNoteAsync()
        {
            const long id = 0;
            await _controller.GetAsync(id);
            _noteServiceMock.Verify(s => s.GetNoteAsync(id), Times.Once);
        }
        
        [Fact]
        public async Task GetAsync_ById_WhenNoteService_GetNoteAsync_ReturnsNull_ShouldReturnNotFound()
        {
            _noteServiceMock
                .Setup(s => s.GetNoteAsync(It.IsAny<long>()))
                .ReturnsAsync(null as Note);
            
            var response = await _controller.GetAsync(It.IsAny<long>());
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAsync_ById_WhenNoteService_GetNoteAsync_ReturnsNote_ShouldReturnNote()
        {
            var initial = new Note { Id = 1, Body = "Note 1" };
            var expected = new Note { Id = initial.Id, Body = initial.Body };

            _noteServiceMock
                .Setup(s => s.GetNoteAsync(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var response = await _controller.GetAsync(It.IsAny<long>());
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var value = response.Result.As<OkObjectResult>().Value;
            value.Should().NotBeNull().And.BeOfType<Note>().Which.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostAsync_WhenInvalidNote_ShouldNotCall_NoteService_AddNoteAsync()
        {
            await _controller.PostAsync(null);
            _noteServiceMock.Verify(s => s.AddNoteAsync(It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public async Task PostAsync_WhenInvalidNote_ShouldReturnBadRequest()
        {
            var response = await _controller.PostAsync(null);
            response.Should().NotBeNull().And.BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PostAsync_WhenValidNote_ShouldCall_NoteService_AddNote()
        {
            var note = new Note { Body = "Note 1" };
            
            _noteServiceMock
                .Setup(s => s.AddNoteAsync(It.IsAny<Note>()))
                .ReturnsAsync(new Note());
            
            await _controller.PostAsync(note);
            _noteServiceMock.Verify(s => s.AddNoteAsync(note), Times.Once);
        }

        [Fact]
        public async Task PostAsync_WhenValidNote_ShouldReturnCreatedAtRoute()
        {
            var initial = new Note { Id = 1, Body = "Note 1" };
            var expected = new Note { Id = initial.Id, Body = initial.Body };
            
            _noteServiceMock
                .Setup(s => s.AddNoteAsync(It.IsAny<Note>()))
                .ReturnsAsync(initial);

            var response = await _controller.PostAsync(initial);
            response.Should().NotBeNull().And.BeOfType<CreatedAtRouteResult>();

            var actual = response.As<CreatedAtRouteResult>();
            actual.RouteValues.Should().Contain("id", expected.Id);
            actual.Value.Should().NotBeNull().And.BeEquivalentTo(expected);
        }
    }
}
