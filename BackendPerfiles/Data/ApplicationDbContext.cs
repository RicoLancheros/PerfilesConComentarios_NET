using Microsoft.EntityFrameworkCore;
using BackendPerfiles.Models;

namespace BackendPerfiles.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Perfil> Perfiles { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones entre Perfil y Comentario
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Perfil)
                .WithMany(p => p.ComentariosRecibidos)
                .HasForeignKey(c => c.PerfilId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.UsuarioComentario)
                .WithMany(p => p.ComentariosRealizados)
                .HasForeignKey(c => c.UsuarioComentarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices para mejorar performance
            modelBuilder.Entity<Perfil>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Comentario>()
                .HasIndex(c => c.PerfilId);

            modelBuilder.Entity<Comentario>()
                .HasIndex(c => c.UsuarioComentarioId);
        }
    }
}
