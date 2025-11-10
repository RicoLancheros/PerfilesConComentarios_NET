using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendPerfiles.Data;
using BackendPerfiles.Models;

namespace BackendPerfiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult<Perfil>> Register([FromBody] Perfil perfil)
        {
            if (_context.Perfiles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Perfiles' is null.");
            }

            // Verificar si el email ya existe
            if (await _context.Perfiles.AnyAsync(p => p.Email == perfil.Email))
            {
                return BadRequest("El email ya está registrado.");
            }

            // Hash de password simple (en producción usar BCrypt o similar)
            perfil.Password = BCrypt.Net.BCrypt.HashPassword(perfil.Password);
            perfil.FechaRegistro = DateTime.Now;

            _context.Perfiles.Add(perfil);
            await _context.SaveChangesAsync();

            // No devolver el password
            perfil.Password = string.Empty;

            return CreatedAtAction("GetPerfil", "Perfiles", new { id = perfil.Id }, perfil);
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            if (_context.Perfiles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Perfiles' is null.");
            }

            var perfil = await _context.Perfiles.FirstOrDefaultAsync(p => p.Email == request.Email);

            if (perfil == null || !BCrypt.Net.BCrypt.Verify(request.Password, perfil.Password))
            {
                return Unauthorized("Credenciales inválidas.");
            }

            // Generar JWT Token
            var token = GenerateJwtToken(perfil);

            return Ok(new
            {
                token,
                perfil = new
                {
                    perfil.Id,
                    perfil.Nombre,
                    perfil.Email,
                    perfil.Telefono,
                    perfil.Direccion,
                    perfil.Foto,
                    perfil.FechaRegistro
                }
            });
        }

        private string GenerateJwtToken(Perfil perfil)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");
            var expirationMinutes = jwtSettings.GetValue<int>("ExpirationMinutes");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, perfil.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, perfil.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("nombre", perfil.Nombre)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
