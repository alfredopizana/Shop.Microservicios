using Account.Api.DTO;
using Account.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Account.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ShopUser> userManager;
        private readonly SignInManager<ShopUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(IConfiguration configuration, UserManager<ShopUser> userManager, 
            SignInManager<ShopUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            if(!await UserExists(loginDTO.UserName.ToLower()))
                return Unauthorized();
            var user = await userManager.Users.SingleAsync(x=> x.UserName == loginDTO.UserName.ToLower());

            var result = await signInManager.CheckPasswordSignInAsync(user,loginDTO.Password, true);
            if(!result.Succeeded)
                return Unauthorized();
            return new UserDTO
            {
                UserName = loginDTO.UserName,
                Token = await GetToken(user)
            };
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.UserName.ToLower()))
                return BadRequest("el usuario ya existe");

            var user = new ShopUser()
            {
                UserName = registerDTO.UserName.ToLower(),
                Email = registerDTO.Email
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                UserName = registerDTO.UserName,
                Token = await GetToken(user)
            };
        }


        private async Task<string> GetToken(ShopUser user)
        {
            var now = DateTime.UtcNow;
            var key = configuration.GetValue<string>("Identity:Key");


            var claims = new List<Claim>
            {
             new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
             new Claim(JwtRegisteredClaimNames.Jti,user.Id),
             new Claim(JwtRegisteredClaimNames.Iat,now.ToUniversalTime().ToString()),
             new Claim(JwtRegisteredClaimNames.Email,user.Email),
            };

            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var signinKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature)
            };


            var encodedJwt = new JwtSecurityTokenHandler();

            var token = encodedJwt.CreateToken(tokenDescriptor);

            return encodedJwt.WriteToken(token);

        }

        private async Task<bool> UserExists(string userName)
            => await userManager.Users.AnyAsync(x=>x.UserName == userName);
        
    }
}
