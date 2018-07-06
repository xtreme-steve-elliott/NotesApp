using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;
using NotesApp.Services;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAsync()
        {
            return Ok(await _noteService.GetNotesAsync());
        }

        [HttpGet]
        [Route("{id}", Name = "GetNoteById")]
        public async Task<ActionResult<Note>> GetAsync(long id)
        {
            var foundNote = await _noteService.GetNoteAsync(id);
            if (foundNote == null)
            {
                return NotFound();
            }

            return Ok(foundNote);
        }
    }
}
