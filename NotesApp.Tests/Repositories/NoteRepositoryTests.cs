using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NotesApp.Models;
using NotesApp.Repositories;
using Xunit;

namespace NotesApp.Tests.Repositories
{
    public class NoteRepositoryTests
    {
        private readonly NoteRepository _repository;
        
        public NoteRepositoryTests()
        {
            _repository = new NoteRepository();
        }
        
        [Fact]
        public async Task GetNotesAsync_ShouldReturnNoteList()
        {
            var expected = new List<Note>
            {
                new Note { Id = 1, Body = "Note 1" }
            };

            var response = await _repository.GetNotesAsync();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}
