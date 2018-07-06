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

        [Fact]
        public async Task GetNote_ById_ShouldCallGetNote()
        {
            const long id = 0;
            await _service.GetNote(id);
            _mockNoteRepository.Verify(r => r.GetNote(id), Times.Once);
        }

        [Fact]
        public async Task GetNote_ById_ShouldReturnNote()
        {
            var initial = new Note {Id = 1, Body = "Note 1"};
            var expected = new Note {Id = initial.Id, Body = initial.Body};

            _mockNoteRepository
                .Setup(r => r.GetNote(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var actual = await _service.GetNote(It.IsAny<long>());
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNote_ShouldCallAddNote()
        {
            var initial = new Note();
            await _service.AddNote(initial);
            _mockNoteRepository.Verify(r => r.AddNote(initial), Times.Once);
        }
        
        [Fact]
        public async Task AddNote_ShouldReturnNoteThatWasSaved()
        {
            var initial = new Note {Id = 1, Body = "Note 1"};
            var expected = new Note {Id = initial.Id, Body = initial.Body};

            _mockNoteRepository
                .Setup(r => r.AddNote(It.IsAny<Note>()))
                .ReturnsAsync(initial);

            var actual = await _service.AddNote(It.IsAny<Note>());
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }
    }
}