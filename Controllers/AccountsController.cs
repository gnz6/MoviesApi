using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.DTOs;
using MoviesApi.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : CustomBaseController
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly MoviesApiDbContext context;



        public AccountsController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> singInManager,
            IConfiguration configuration,
            MoviesApiDbContext context, IMapper mapper) : base(context, mapper)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _configuration = configuration;
            this.context = context;
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                return await GenerateToken(model);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email , model.Password, isPersistent: false, lockoutOnFailure: false);

            if( result.Succeeded)
            {
                return await GenerateToken(model);
            }
            else
            {
                return BadRequest("Invalid Login attempt");
            }
        }

        [HttpPost("RenewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renovar()
        {
            var userInfo = new UserInfo
            {
                Email = HttpContext.User.Identity.Name
            };

            return await GenerateToken(userInfo);
        }

        private async Task<UserToken> GenerateToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email),
            };

            var identityUser = await _userManager.FindByEmailAsync(userInfo.Email);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
issuer: null,
audience: null,
claims: claims,
                expires: expiracion,
            signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };

        }

        [HttpGet("Users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PagingDTO paginationDTO)
        {
            var queryable = context.Users.AsQueryable();
            queryable = queryable.OrderBy(x => x.Email);
            return await Get<IdentityUser, UserDTO>(paginationDTO);
        }

        [HttpGet("Roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<string>>> GetRoles()
        {
            return await context.Roles.Select(x => x.Name).ToListAsync();
        }

        [HttpPost("AssignRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> AsignarRol(EditRoleDTO editarRolDTO)
        {
            var user = await _userManager.FindByIdAsync(editarRolDTO.UsuarioId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.NombreRol));
            return NoContent();
        }

        [HttpPost("RemoveRol")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> RemoverRol(EditRoleDTO editarRolDTO)
        {
            var user = await _userManager.FindByIdAsync(editarRolDTO.UsuarioId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editarRolDTO.NombreRol));
            return NoContent();
        }


    }
}
