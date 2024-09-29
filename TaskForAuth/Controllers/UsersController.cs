using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskForAuth.Data.Models;
using TaskForAuth.ViewModels;

namespace TaskForAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<SchoolUser> _userManager;
        private readonly SymmetricSecurityKey _key;

        public UsersController(UserManager<SchoolUser> userManager , SymmetricSecurityKey key)
        {
            _userManager = userManager;
            _key = key;
        }

        [HttpPost]
        [Route("RegisterForStudent")]
        public async Task<ActionResult> RegisterAsStudent(RegisterVM model)
        {
            var student = new SchoolUser()
            {
                UserName = model.UserName,
                SchoolName = model.SchoolName,
            };
            var result = await _userManager.CreateAsync(student, model.Password);
            if (!result.Succeeded)
                return BadRequest();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, student.Id),
                new Claim(ClaimTypes.Email,student.Email),
                new Claim(ClaimTypes.Role,"Student")

            };
            var RolesResult = await _userManager.AddClaimsAsync(student, claims);

            if (!RolesResult.Succeeded)
                return BadRequest();

            return Ok();
        }
        [HttpPost]
        [Route("RegisterForTeacher")]

        public async Task<ActionResult> RegisterAsTeacher(RegisterVM model)
        {
            var teacher = new SchoolUser()
            {
                UserName = model.UserName,
                SchoolName = model.SchoolName,
            };
            var result = await _userManager.CreateAsync(teacher, model.Password);
            if (!result.Succeeded)
                return BadRequest();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, teacher.Id),
                new Claim(ClaimTypes.Email,teacher.Email),
                new Claim(ClaimTypes.Role,"Teacher")

            };
            var RolesResult = await _userManager.AddClaimsAsync(teacher, claims);

            if (!RolesResult.Succeeded)
                return BadRequest();

            return Ok();
        }

        [HttpPost]
        [Route("Login")]

        public async Task<ActionResult> Login(LoginVM model)
        {
            var schoolUser = await _userManager.FindByNameAsync(model.UserName);

            if (!await _userManager.CheckPasswordAsync(schoolUser, model.Password))
                return Unauthorized();

            var claims = await _userManager.GetClaimsAsync(schoolUser);

            var signingCredential = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredential,
                expires: DateTime.Now.AddMinutes(30),
                notBefore: DateTime.Now
                );

            var tokenHandeler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandeler.WriteToken(jwt);

            return Ok(new
            {
                Token = tokenString,
                Expire = DateTime.Now.AddMinutes(30)
            });
        }
    }
}
