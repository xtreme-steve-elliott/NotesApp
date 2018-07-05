using System.Collections.Generic;
using System.Linq;
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
            Assert.NotNull(response);
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);

            var value = ((OkObjectResult) response.Result).Value;
            Assert.NotNull(value);
            Assert.IsAssignableFrom<IEnumerable<Note>>(value);

            var actual = ((IEnumerable<Note>) value).ToList();
            Assert.NotNull(actual);
            Assert.NotEmpty(actual);
            Assert.Equal(actual.Count, expected.Count);
            Assert.Equal(actual[0].Id, expected[0].Id);
            Assert.Equal(actual[0].Body, expected[0].Body);
        }
    }
}
