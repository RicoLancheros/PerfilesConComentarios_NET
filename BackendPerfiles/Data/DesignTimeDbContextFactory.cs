using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BackendPerfiles.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Leer contraseña desde variable de entorno
            var mysqlPassword = Environment.GetEnvironmentVariable("MySQL_PSS") ?? "";
            var connectionString = $"Server=localhost;Database=PerfilesDB;User=root;Password='{mysqlPassword}';";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 21))
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
