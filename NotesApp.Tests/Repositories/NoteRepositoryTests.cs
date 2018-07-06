using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NotesApp.Models;
using NotesApp.Repositories;
using Xunit;

namespace NotesApp.Tests.Repositories
{
    public class NoteRepositoryTests : IDisposable
    {
        private static int _testDbIndex;
        
        private readonly NotesAppDbContext _context;
        private readonly NoteRepository _repository;
        
        public NoteRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<NotesAppDbContext>()
                .UseInMemoryDatabase($"note_repository_tests{_testDbIndex++}")
                .Options;
            _context = new NotesAppDbContext(dbContextOptions);
            _repository = new NoteRepository(_context);
        }

        public void Dispose()
        {
            _context?.Database?.EnsureDeleted();
            _context?.Dispose();
        }

        [Fact]
        public async Task GetNotes_WhenNoNotes_ShouldReturnEmptyList()
        {
            var response = await _repository.GetNotes();
            response.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public async Task GetNotes_WhenNotes_ShouldReturnNoteList()
        {
            var initial = new Note {Id = 1, Body = "Note 1"};
            var expected = new List<Note>
            {
                new Note {Id = initial.Id, Body = initial.Body}
            };

            await _repository.AddNote(initial);
            
            var response = await _repository.GetNotes();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetNote_ById_WhenInvalidId_ShouldReturnNull()
        {
            var actual = await _repository.GetNote(It.IsAny<long>());
            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetNote_ById_WhenValidId_ShouldReturnNote()
        {
            var initial = new Note {Id = 1, Body = "Note 1"};
            var expected = new Note {Id = initial.Id, Body = initial.Body};

            await _repository.AddNote(initial);

            var actual = await _repository.GetNote(initial.Id);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddNote_ShouldSaveNote()
        {
            var initial = new Note {Body = "Note 1"};
            var expected = new List<Note>
            {
                new Note {Body = initial.Body}
            };

            var beforeGetNotesResponse = await _repository.GetNotes();
            beforeGetNotesResponse.Should().NotBeNull().And.BeEmpty();
            
            await _repository.AddNote(initial);

            var afterGetNotesResponse = await _repository.GetNotes();
            // Note: Due to https://github.com/aspnet/EntityFrameworkCore/issues/6872 we can't assume a particular id
            afterGetNotesResponse.Should().NotBeNull().And.BeEquivalentTo(expected, o => o.Excluding(n => n.Id));
        }
        
        [Fact]
        public async Task AddNote_ShouldReturnNoteThatWasSaved()
        {
            var initial = new Note {Body = "Note 1"};
            var expected = new Note {Body = initial.Body};

            var actual = await _repository.AddNote(initial);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected, o => o.Excluding(n => n.Id));
            // Note: Due to https://github.com/aspnet/EntityFrameworkCore/issues/6872 we can't assume a particular id
            actual.Id.Should().BePositive();
        }

        [Fact]
        public async Task DeleteNote_ById_WhenInvalidId_ShouldReturnFalse()
        {
            var actual = await _repository.DeleteNote(It.IsAny<long>());
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteNote_ById_WhenValidId_ShouldReturnTrue()
        {
            var initial = new Note {Id = 1, Body = "Note 1"};

            await _repository.AddNote(initial);

            var actual = await _repository.DeleteNote(initial.Id);
            actual.Should().BeTrue();
        }
    }
}