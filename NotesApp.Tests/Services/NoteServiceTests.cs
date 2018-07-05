﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NotesApp.Models;
using NotesApp.Repositories;
using NotesApp.Services;
using Xunit;

namespace NotesApp.Tests.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _noteRepositoryMock;
        private readonly NoteService _service;
        
        public NoteServiceTests()
        {
            _noteRepositoryMock = new Mock<INoteRepository>();
            _service = new NoteService(_noteRepositoryMock.Object);
        }

        [Fact]
        public async Task GetNotesAsync_ShouldCall_NoteRepository_GetNotesAsync()
        {
            await _service.GetNotesAsync();
            _noteRepositoryMock.Verify(r => r.GetNotesAsync(), Times.Once);
        }
        
        [Fact]
        public async Task GetNotesAsync_ShouldReturn_NoteRepository_GetNotesAsync()
        {
            var initial = new List<Note>
            {
                new Note { Id = 1, Body = "Note 1" }
            };
            var expected = initial;

            _noteRepositoryMock
                .Setup(r => r.GetNotesAsync())
                .ReturnsAsync(initial);

            var response = await _service.GetNotesAsync();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}