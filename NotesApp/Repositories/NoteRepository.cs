using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApp.Models;

namespace NotesApp.Repositories
{
    public class NoteRepository : INoteRepository
    {
        public Task<IEnumerable<Note>> GetNotes()
        {
            return Task.FromResult(new[] {new Note {Id = 1, Body = "Note 1"}}.AsEnumerable());
        }
    }
}