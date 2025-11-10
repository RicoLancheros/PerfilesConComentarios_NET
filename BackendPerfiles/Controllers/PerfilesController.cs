using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BackendPerfiles.Data;
using BackendPerfiles.Models;

namespace BackendPerfiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PerfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Perfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPerfiles()
        {
            if (_context.Perfiles == null)
            {
                return NotFound();
            }

            var perfiles = await _context.Perfiles
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Telefono,
                    p.Direccion,
                    p.Email,
                    p.Foto,
                    p.FechaRegistro
                })
                .ToListAsync();

            return Ok(perfiles);
        }

        // GET: api/Perfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetPerfil(int id)
        {
            if (_context.Perfiles == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfiles
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Telefono,
                    p.Direccion,
                    p.Email,
                    p.Foto,
                    p.FechaRegistro
                })
                .FirstOrDefaultAsync();

            if (perfil == null)
            {
                return NotFound();
            }

            return Ok(perfil);
        }

        // PUT: api/Perfiles/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPerfil(int id, Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return BadRequest();
            }

            // Obtener el perfil existente
            var existingPerfil = await _context.Perfiles.FindAsync(id);
            if (existingPerfil == null)
            {
                return NotFound();
            }

            // Actualizar solo los campos permitidos
            existingPerfil.Nombre = perfil.Nombre;
            existingPerfil.Telefono = perfil.Telefono;
            existingPerfil.Direccion = perfil.Direccion;

            if (!string.IsNullOrEmpty(perfil.Foto))
            {
                existingPerfil.Foto = perfil.Foto;
            }

            // Si se envía una nueva contraseña, hashearla
            if (!string.IsNullOrEmpty(perfil.Password))
            {
                existingPerfil.Password = BCrypt.Net.BCrypt.HashPassword(perfil.Password);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Perfiles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePerfil(int id)
        {
            if (_context.Perfiles == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfiles.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            _context.Perfiles.Remove(perfil);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerfilExists(int id)
        {
            return (_context.Perfiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
