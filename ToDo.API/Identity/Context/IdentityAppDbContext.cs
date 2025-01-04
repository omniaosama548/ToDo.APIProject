using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Context;
using ToDo.API.Identity.Models;

namespace ToDo.API.Identity.Context
{
    public class IdentityAppDbContext :IdentityDbContext<AppUser>
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> Options) : base(Options)
        {

        }
    }
}
