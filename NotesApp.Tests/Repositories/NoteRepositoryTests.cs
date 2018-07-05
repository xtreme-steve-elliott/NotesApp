using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
            var expected = new List<Note>
            {
                new Note {Id = 1, Body = "Note 1"}
            };

            { // Setup for test until we get around to having an Add method in the service
                _context.Notes.Add(expected[0]);
                _context.SaveChanges();
            }
            
            var response = await _repository.GetNotes();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}