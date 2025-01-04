using Microsoft.AspNetCore.Identity;
using ToDo.API.Identity.Models;
using ToDo.API.Models;

namespace ToDo.API.Services
{
    public interface ITokenServices
    {
        Task<string> CreateTokenAsync(User User);
    }
}
