using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendPerfiles.Models
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Contenido { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // ID del perfil donde se hizo el comentario (el perfil que recibe el comentario)
        [Required]
        public int PerfilId { get; set; }

        [ForeignKey("PerfilId")]
        public virtual Perfil? Perfil { get; set; }

        // ID del usuario que hizo el comentario
        [Required]
        public int UsuarioComentarioId { get; set; }

        [ForeignKey("UsuarioComentarioId")]
        public virtual Perfil? UsuarioComentario { get; set; }
    }
}
