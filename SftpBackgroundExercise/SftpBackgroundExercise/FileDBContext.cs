using Microsoft.EntityFrameworkCore;

namespace SftpBackgroundExercise
{
    public class FileDBContext : DbContext
    {
        public FileDBContext(DbContextOptions<FileDBContext> options) : base(options) { }
        public DbSet<FileRecord> Files { get; set; }
    }
}