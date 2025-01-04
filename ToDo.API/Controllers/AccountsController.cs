using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDo.API.Context;
using ToDo.API.DTOs;
using ToDo.API.Error;
using ToDo.API.Identity.Models;
using ToDo.API.Models;
using ToDo.API.Repositories;
using ToDo.API.Services;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ITokenServices _tokenServices;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _dbContext;
        private readonly IGenericRepository<User> _userRepo;
        private readonly List<string> _revokedTokens = new List<string>();

        public AccountsController(ITokenServices tokenServices,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            AppDbContext dbContext,
            IGenericRepository<User> userRepo)
        {
            _tokenServices = tokenServices;
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _userRepo = userRepo;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO model)
        {

            if (_dbContext.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest(new ApiResponse(400,"Email Already Exist"));
            }
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = new User()
            {
                Email = model.Email,
                Username = model.UserName,
                Password = model.Password,
            };
            await _userRepo.AddAsync(user);
            var ReturnedUser = new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user)
            };
            return Ok(new
            {
                data = new
                {
                    user = new
                    {
                        email = user.Email,
                        username = user.Username,
                        id = user.Id
                    },
                    token = ReturnedUser.Token
                }
            });
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized(new ApiResponse(401,"Invalid Information"));
            }
            var ReturnedUser = new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user)
            };
            return Ok(new
            {
                data = new
                {
                    user = new
                    {
                        email = user.Email,
                        username = user.Username,
                        id = user.Id
                    },
                    token = ReturnedUser.Token
                }
            });
        }


    }
}
