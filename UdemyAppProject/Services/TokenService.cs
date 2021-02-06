using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UdemyAppProject.Entities;
using UdemyAppProject.Interfaces;

namespace UdemyAppProject.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateTokoen(AppUser user)
        {

            //STEP:1
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName)
            };

            //STEP:2
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //STEP:3
            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject=new ClaimsIdentity(claims),
               Expires=DateTime.Now.AddMinutes(2),
               SigningCredentials=credentials,
            };

            //STEP:4
            var tokenHandler = new JwtSecurityTokenHandler();

            //STEP:5
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //STEP:6
            return tokenHandler.WriteToken(token);
        }
    }
}
