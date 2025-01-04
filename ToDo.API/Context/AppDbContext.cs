using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ToDo.API.Models;

namespace ToDo.API.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>Options):base(Options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<DoTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
