using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApp.Models;

namespace NotesApp.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotesAsync();
        Task<Note> GetNoteAsync(long id);
        Task<Note> AddNoteAsync(Note note);
        Task<bool> DeleteNoteAsync(long id);
    }
}
