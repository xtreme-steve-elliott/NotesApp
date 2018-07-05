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
        private readonly Mock<INoteService> _mockNoteService;
        private readonly NotesController _controller;
        
        public NotesControllerTests()
        {
            _mockNoteService = new Mock<INoteService>();
            _controller = new NotesController(_mockNoteService.Object);
        }

        [Fact]
        public async Task Get_ShouldCallGetNotes()
        {
            await _controller.Get();
            _mockNoteService.Verify(s => s.GetNotes(), Times.Once);
        }
        
        [Fact]
        public async Task Get_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new Note {Id = 1, Body = "Note 1"}
            };
            var expected = initial;

            _mockNoteService
                .Setup(s => s.GetNotes())
                .ReturnsAsync(initial);

            var response = await _controller.Get();
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var actual = response.Result.As<OkObjectResult>().Value?.As<IEnumerable<Note>>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}