﻿using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Note>>> GetAsync()
        {
            return Ok(await _noteService.GetNotesAsync());
        }

        [HttpGet]
        [Route("{id}", Name = "GetNoteById")]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Note), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Note>> GetAsync([FromRoute] long id)
        {
            var foundNote = await _noteService.GetNoteAsync(id);
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
        public async Task<ActionResult> PostAsync([FromBody] Note note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            var processedNote = await _noteService.AddNoteAsync(note);
            return CreatedAtRoute("GetNoteById", new { id = processedNote.Id }, processedNote);
        }

        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        {
            if (await _noteService.DeleteNoteAsync(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
