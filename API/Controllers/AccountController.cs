using API.HandleReponses;
using Core.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.services.TokenService;
using Services.services.UserService;
using Services.services.UserService.Dto;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;

        public AccountController(
            IUserService userService,
             UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            _userService = userService;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await _userService.Register(registerDto);

            if(user == null)
            {
                return BadRequest(new APIException(400, "Email Already Exists"));

            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.Login(loginDto);

            if(user == null)
            {
                return Unauthorized(new APIResponse(401));
            }

            return Ok(user);
        }

        [HttpGet("getCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> getCurrentUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailAsync(email);

            return new UserDto
            {
                DisplayName = user.Displayname,
                Email = user.Email
            };
        }
    }
}
