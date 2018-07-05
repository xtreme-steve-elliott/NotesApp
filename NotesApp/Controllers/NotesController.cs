using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;

namespace NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAsync()
        {
            return await Task.FromResult(Ok(new[] { new Note { Id = 1, Body = "Note 1" } }));
        }
    }
}
