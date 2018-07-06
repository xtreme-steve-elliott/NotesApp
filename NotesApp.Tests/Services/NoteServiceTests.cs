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
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly NoteService _service;
        
        public NoteServiceTests()
        {
            _noteRepositoryMock = new Mock<INoteRepository>();
            _service = new NoteService(_noteRepositoryMock.Object);
        }

        [Fact]
        public async Task GetNotesAsync_ShouldCall_NoteRepository_GetNotesAsync()
        {
            await _service.GetNotesAsync();
            _noteRepositoryMock.Verify(r => r.GetNotesAsync(), Times.Once);
        }
        
        [Fact]
        public async Task GetNotesAsync_ShouldReturn_NoteRepository_GetNotesAsync()
        {
            var initial = new List<Note>
            {
                new Note { Id = 1, Body = "Note 1" }
            };
            var expected = initial;

            _noteRepositoryMock
                .Setup(r => r.GetNotesAsync())
                .ReturnsAsync(initial);

            var response = await _service.GetNotesAsync();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetNoteAsync_ById_ShouldCall_NoteRepository_GetNoteAsync()
        {
            const long id = 0;
            await _service.GetNoteAsync(id);
            _noteRepositoryMock.Verify(r => r.GetNoteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetNoteAsync_ById_ShouldReturn_NoteRepository_GetNoteAsync()
        {
            var initial = new Note { Id = 1, Body = "Note 1" };
            var expected = new Note { Id = initial.Id, Body = initial.Body };

            _noteRepositoryMock
                .Setup(r => r.GetNoteAsync(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var actual = await _service.GetNoteAsync(It.IsAny<long>());
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNoteAsync_ShouldCall_NoteRepository_AddNoteAsync()
        {
            var initial = new Note();
            await _service.AddNoteAsync(initial);
            _noteRepositoryMock.Verify(r => r.AddNoteAsync(initial), Times.Once);
        }
        
        [Fact]
        public async Task AddNoteAsync_ShouldReturn_NoteRepository_AddNoteAsync()
        {
            var initial = new Note { Id = 1, Body = "Note 1" };
            var expected = new Note { Id = initial.Id, Body = initial.Body };

            _noteRepositoryMock
                .Setup(r => r.AddNoteAsync(It.IsAny<Note>()))
                .ReturnsAsync(initial);

            var actual = await _service.AddNoteAsync(It.IsAny<Note>());
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteNoteAsync_ById_ShouldCall_NoteService_DeleteNoteAsync()
        {
            const long id = 0;
            await _service.DeleteNoteAsync(id);
            _noteRepositoryMock.Verify(r => r.DeleteNoteAsync(id), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteNoteAsync_ById_ShouldReturn_NoteService_DeleteNoteAsync(bool success)
        {
            _noteRepositoryMock
                .Setup(r => r.DeleteNoteAsync(It.IsAny<long>()))
                .ReturnsAsync(success);

            var actual = await _service.DeleteNoteAsync(It.IsAny<long>());
            actual.Should().Be(success);
        }
    }
}
