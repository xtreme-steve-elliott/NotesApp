﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApp.Controllers;
using NotesApp.Models;
using NotesApp.Services;
using Xunit;

namespace NotesApp.Tests.Controllers
{
    public class NotesControllerTests
    {
        private readonly Mock<INoteService> _mockNoteService;
        private readonly NotesController _controller;
        
        public NotesControllerTests()
        {
            _mockNoteService = new Mock<INoteService>();
            _controller = new NotesController(_mockNoteService.Object);
        }

        [Fact]
        public async Task Get_ShouldCallGetNotes()
        {
            await _controller.Get();
            _mockNoteService.Verify(s => s.GetNotes(), Times.Once);
        }
        
        [Fact]
        public async Task Get_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new Note {Id = 1, Body = "Note 1"}
            };
            var expected = initial;

            _mockNoteService
                .Setup(s => s.GetNotes())
                .ReturnsAsync(initial);

            var response = await _controller.Get();
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var actual = response.Result.As<OkObjectResult>().Value?.As<IEnumerable<Note>>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Get_ById_ShouldCallGetNote()
        {
            const long id = 0;
            await _controller.Get(id);
            _mockNoteService.Verify(s => s.GetNote(id), Times.Once);
        }
        
        [Fact]
        public async Task Get_ById_WhenNoteNull_ShouldReturnNotFound()
        {
            _mockNoteService
                .Setup(s => s.GetNote(It.IsAny<long>()))
                .ReturnsAsync(null as Note);
            
            var response = await _controller.Get(It.IsAny<long>());
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_ById_WhenFoundNote_ShouldReturnNote()
        {
            var initial = new Note {Id = 1, Body = "Note 1"};
            var expected = new Note {Id = initial.Id, Body = initial.Body};

            _mockNoteService
                .Setup(s => s.GetNote(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var response = await _controller.Get(It.IsAny<long>());
            response.Should().NotBeNull();
            response.Result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var value = response.Result.As<OkObjectResult>().Value;
            value.Should().NotBeNull().And.BeOfType<Note>().Which.Should().BeEquivalentTo(expected);
        }
    }
}