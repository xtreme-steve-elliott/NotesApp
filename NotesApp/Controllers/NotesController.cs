﻿using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Note>>> Get()
        {
            return Ok(await _noteService.GetNotes());
        }

        [HttpGet]
        [Route("{id}", Name = "GetNoteById")]
        public async Task<ActionResult<Note>> Get(long id)
        {
            var foundNote = await _noteService.GetNote(id);
            if (foundNote == null)
            {
                return NotFound();
            }

            return Ok(foundNote);
        }

        [HttpPost]
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