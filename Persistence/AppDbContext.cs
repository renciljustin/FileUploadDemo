using Microsoft.EntityFrameworkCore;
using UploadFileDemo.Core.Models;

namespace UploadFileDemo.Persistence
{
    public class AppDbContext: DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
    }
}