using API_PUNTUALCHECK.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace API_PUNTUALCHECK.Utils
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Representante> Representantes => Set<Representante>();
        public DbSet<EstudianteRepresentante> EstudiantesRepresentantes => Set<EstudianteRepresentante>();
        public DbSet<Horario> Horarios => Set<Horario>();
        public DbSet<Asistencia> Asistencias => Set<Asistencia>();
        public DbSet<Notificacion> Notificaciones => Set<Notificacion>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.HasDefaultSchema("Main");

            mb.Entity<Usuario>().ToTable("usuarios");
            mb.Entity<Estudiante>().ToTable("estudiantes");
            mb.Entity<Representante>().ToTable("representantes");
            mb.Entity<EstudianteRepresentante>().ToTable("estudiante_representante");
            mb.Entity<Horario>().ToTable("horarios");
            mb.Entity<Asistencia>().ToTable("asistencias");
            mb.Entity<Notificacion>().ToTable("notificaciones");
        }

    }
}
