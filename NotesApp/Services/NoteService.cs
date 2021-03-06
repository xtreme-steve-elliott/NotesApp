﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApp.Models;
using NotesApp.Repositories;

namespace NotesApp.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        
        public Task<IEnumerable<Note>> GetNotesAsync()
        {
            return _noteRepository.GetNotesAsync();
        }

        public Task<Note> GetNoteAsync(long id)
        {
            return _noteRepository.GetNoteAsync(id);
        }

        public Task<Note> AddNoteAsync(Note note)
        {
            return _noteRepository.AddNoteAsync(note);
        }

        public Task<bool> DeleteNoteAsync(long id)
        {
            return _noteRepository.DeleteNoteAsync(id);
        }
    }
}
