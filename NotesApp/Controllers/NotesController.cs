using System.Collections.Generic;
using System.Net;
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Note>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Note>>> Get()
        {
            return Ok(await _noteService.GetNotes());
        }

        [HttpGet]
        [Route("{id}", Name = "GetNoteById")]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Note), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Note>> Get([FromRoute] long id)
        {
            var foundNote = await _noteService.GetNote(id);
            if (foundNote == null)
            {
                return NotFound();
            }

            return Ok(foundNote);
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Note), (int) HttpStatusCode.Created)]
        public async Task<ActionResult> Post([FromBody] Note note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            var processedNote = await _noteService.AddNote(note);
            return CreatedAtRoute("GetNoteById", new {id = processedNote.Id}, processedNote);
        }

        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(long id)
        {
            if (await _noteService.DeleteNote(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}