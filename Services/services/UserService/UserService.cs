using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Services.services.TokenService;
using Services.services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.services.UserService
{
    public class UserService : IUserService
    {
        private  UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, ITokenService tokenService,  SignInManager<AppUser>  signInManager) {

            _userManager = userManager;
            _tokenService = tokenService;
           _signInManager = signInManager;
        }
        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);

            if (user != null)
            {
                return null;
            }

            var appUser = new AppUser
            {
                Displayname = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0]
            };

            var result = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            return new UserDto
            {
                DisplayName = appUser.Displayname,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser)
            };
            }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if(!result.Succeeded)
            {
                return null;
            }

            return new UserDto
            {
                DisplayName = user.Displayname,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }


    }

    }




