using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BackendPerfiles.Data;
using BackendPerfiles.Models;

namespace BackendPerfiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComentariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comentarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetComentarios()
        {
            if (_context.Comentarios == null)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios
                .Include(c => c.UsuarioComentario)
                .Include(c => c.Perfil)
                .Select(c => new
                {
                    c.Id,
                    c.Contenido,
                    c.FechaCreacion,
                    c.PerfilId,
                    c.UsuarioComentarioId,
                    UsuarioComentario = new
                    {
                        c.UsuarioComentario!.Id,
                        c.UsuarioComentario.Nombre,
                        c.UsuarioComentario.Foto
                    }
                })
                .ToListAsync();

            return Ok(comentarios);
        }

        // GET: api/Comentarios/ByPerfil/5
        [HttpGet("ByPerfil/{perfilId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetComentariosByPerfil(int perfilId)
        {
            if (_context.Comentarios == null)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios
                .Where(c => c.PerfilId == perfilId)
                .Include(c => c.UsuarioComentario)
                .OrderByDescending(c => c.FechaCreacion)
                .Select(c => new
                {
                    c.Id,
                    c.Contenido,
                    c.FechaCreacion,
                    c.PerfilId,
                    c.UsuarioComentarioId,
                    UsuarioComentario = new
                    {
                        c.UsuarioComentario!.Id,
                        c.UsuarioComentario.Nombre,
                        c.UsuarioComentario.Foto
                    }
                })
                .ToListAsync();

            return Ok(comentarios);
        }

        // GET: api/Comentarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetComentario(int id)
        {
            if (_context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.UsuarioComentario)
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Contenido,
                    c.FechaCreacion,
                    c.PerfilId,
                    c.UsuarioComentarioId,
                    UsuarioComentario = new
                    {
                        c.UsuarioComentario!.Id,
                        c.UsuarioComentario.Nombre,
                        c.UsuarioComentario.Foto
                    }
                })
                .FirstOrDefaultAsync();

            if (comentario == null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        // POST: api/Comentarios
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comentario>> PostComentario(Comentario comentario)
        {
            if (_context.Comentarios == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comentarios' is null.");
            }

            // Validar que el perfil y usuario existan
            var perfilExiste = await _context.Perfiles.AnyAsync(p => p.Id == comentario.PerfilId);
            var usuarioExiste = await _context.Perfiles.AnyAsync(p => p.Id == comentario.UsuarioComentarioId);

            if (!perfilExiste || !usuarioExiste)
            {
                return BadRequest("Perfil o usuario no encontrado.");
            }

            comentario.FechaCreacion = DateTime.Now;
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComentario", new { id = comentario.Id }, comentario);
        }

        // PUT: api/Comentarios/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutComentario(int id, Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return BadRequest();
            }

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
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

        // DELETE: api/Comentarios/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            if (_context.Comentarios == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioExists(int id)
        {
            return (_context.Comentarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
