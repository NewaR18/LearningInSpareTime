using LearnFileHandling.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnFileHandling.Database_Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<FileContents> FileContents { get; set; }
    }
}
