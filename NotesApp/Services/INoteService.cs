﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NotesApp.Models;

namespace NotesApp.Services
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetNotesAsync();
    }
}