using Microsoft.EntityFrameworkCore;

namespace NotesApp.Models
{
    public class NotesAppDbContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        
        public NotesAppDbContext(DbContextOptions<NotesAppDbContext> options) : base(options) { }
    }
}
