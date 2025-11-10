using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendPerfiles.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Foto guardada como Base64
        public string? Foto { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Relación con Comentarios - Comentarios que otros han hecho en este perfil
        public virtual ICollection<Comentario> ComentariosRecibidos { get; set; } = new List<Comentario>();

        // Relación con Comentarios - Comentarios que este perfil ha hecho en otros
        public virtual ICollection<Comentario> ComentariosRealizados { get; set; } = new List<Comentario>();
    }
}
