using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApp.Models;

namespace NotesApp.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotes();
        Task<Note> GetNote(long id);
        Task<Note> AddNote(Note note);
        Task<bool> DeleteNote(long id);
    }
}