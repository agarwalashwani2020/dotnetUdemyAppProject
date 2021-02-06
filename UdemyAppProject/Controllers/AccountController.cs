using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UdemyAppProject.Data;
using UdemyAppProject.Entities;
using Microsoft.EntityFrameworkCore;
using UdemyAppProject.DTO;
using UdemyAppProject.Services;
using UdemyAppProject.Interfaces;

namespace UdemyAppProject.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] ResgiterDto registerDto)
        //public async Task<ActionResult<AppUser>> Register(string userName,string password)
        {
            if(IsUserNameExist(registerDto.UserName))
            {
                return BadRequest("username exists");
            }
            
            using var hmac = new HMACSHA512();

            var userToAdd = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _dataContext.Users.Add(userToAdd);
            await _dataContext.SaveChangesAsync();
            return new UserDto 
            { 
                Username=userToAdd.UserName,
                Token=_tokenService.CreateTokoen(userToAdd)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(userData => userData.UserName == loginDto.UserName);
            if(user==null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i]!=user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateTokoen(user)
            };
        }

        private bool IsUserNameExist(string userName)
        {
            return _dataContext.Users.Any(user => user.UserName.Equals(userName.ToLower()));
        }
    }
}
