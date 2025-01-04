using Microsoft.AspNetCore.Identity;

namespace ToDo.API.Identity.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName{ get; set; }
    }
}
