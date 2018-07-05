using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Controllers;
using NotesApp.Models;
using Xunit;

namespace NotesApp.Tests.Controllers
{
    public class NotesControllerTests
    {
        [Fact]
        public void Get_ShouldReturnNoteList()
        {
            var expected = new List<Note>
            {
                new Note { Id = 1, Body = "Note 1" }
            };

            var controller = new NotesController();

            var response = controller.Get();
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var actual = response.Result.As<OkObjectResult>().Value?.As<IEnumerable<Note>>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}
