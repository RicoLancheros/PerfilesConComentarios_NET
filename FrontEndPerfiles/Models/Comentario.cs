namespace FrontEndPerfiles.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int PerfilId { get; set; }
        public int UsuarioComentarioId { get; set; }
        public UsuarioComentario? UsuarioComentario { get; set; }
    }

    public class UsuarioComentario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Foto { get; set; }
    }
}
