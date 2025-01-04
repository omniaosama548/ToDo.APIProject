using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.API.Identity.Migrations;
using ToDo.API.Models;

namespace ToDo.API.Configuration
{
    public class TaskConfig : IEntityTypeConfiguration<DoTask>
    {
        public void Configure(EntityTypeBuilder<DoTask> builder)
        {
            builder.HasOne(T => T.User).WithMany(U => U.Tasks)
                    .HasForeignKey(T => T.UserId);
           builder.Property(e => e.Status)
               .HasConversion(
                   v => (int)v, 
                   v => (TasksStatus)v);
        }
    }
}
