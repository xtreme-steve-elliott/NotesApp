using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotesApp.Models;

namespace NotesApp.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NotesAppDbContext _context;
        
        public NoteRepository(NotesAppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        
        public Task<IEnumerable<Note>> GetNotes()
        {
            return Task.FromResult(_context.Notes.AsEnumerable() ?? Enumerable.Empty<Note>());
        }
    }
}