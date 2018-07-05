using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NotesApp.Models;
using NotesApp.Repositories;
using NotesApp.Services;
using Xunit;

namespace NotesApp.Tests.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly NoteService _service;
        
        public NoteServiceTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _service = new NoteService(_mockNoteRepository.Object);
        }

        [Fact]
        public async Task GetNotes_ShouldCallGetNotes()
        {
            await _service.GetNotes();
            _mockNoteRepository.Verify(r => r.GetNotes(), Times.Once);
        }
        
        [Fact]
        public async Task GetNotes_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new Note {Id = 1, Body = "Note 1"}
            };
            var expected = initial;

            _mockNoteRepository
                .Setup(r => r.GetNotes())
                .ReturnsAsync(initial);

            var response = await _service.GetNotes();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}