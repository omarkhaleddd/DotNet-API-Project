using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Pokimon.Dto;
using Pokimon.Interfaces;
using Pokimon.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Pokimon.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository , IMapper mapper,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        [HttpPost("/Register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody]UserDto user)
        {
            if(user == null)
            {
                return BadRequest();
            }
            var check = await _userRepository.GetUsers();
            var check2 =  check.Where(u => u.Email.Trim().ToUpper() == user.Email.TrimEnd().ToUpper())
                .FirstOrDefault();
            if(check2 != null)
            { 
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CreatePasswordHash(user.Password ,out byte[] PasswordHash ,out byte[] PasswordSalt);

            var userMap = _mapper.Map<User>(user);
            userMap.PasswordHash = PasswordHash;
            userMap.PasswordSalt = PasswordSalt;
            if (! await _userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong!");
                return StatusCode(500 , ModelState);
            }
            return Ok(user.Email);
        }
        [HttpPost("/Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody]UserDto user)
        {
            if (user == null) 
            {
                return BadRequest(ModelState);
            }
            var check = await _userRepository.GetUserByEmail(user.Email);
            if (check == null)
            {
                return NotFound();
            }

            if(!VerifyPasswordHash(user.Password , check.PasswordHash,check.PasswordSalt))
            {
                ModelState.AddModelError("", "Wrong Password");
                return StatusCode(401,ModelState);
            }
            user.Role = (int) check.Role;
            //user token return
            var token = CreateToken(user);
            return Ok(token);
        }
        private bool VerifyPasswordHash(string password , byte[] passwordHash ,byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private void CreatePasswordHash(string Password,out byte[] PasswordHash,out byte[]PasswordSalt) 
        {   
            using(var hmac = new HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }
        //if condtion check if user.role == 1 therefore admin and then create  with admin claims 
        private string CreateToken (UserDto user)
        {
            string userType = "User";
            if (user.Role == 1)
            {
                 userType = "Admin";
            }
            List<Claim> claims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.Email),
               new Claim(ClaimTypes.Role, userType)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds =new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
