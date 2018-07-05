using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Note>> Get()
        {
            return Ok(new[] {new Note {Id = 1, Body = "Note 1"}});
        }
    }
}