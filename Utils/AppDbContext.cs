using API_PUNTUALCHECK.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

<<<<<<< HEAD
=======

>>>>>>> 5fe029b6863df2921e0af858ed485891b07eea34
namespace API_PUNTUALCHECK.Utils
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

<<<<<<< HEAD
        // DbSets existentes
=======
>>>>>>> 5fe029b6863df2921e0af858ed485891b07eea34
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<Representante> Representantes => Set<Representante>();
        public DbSet<EstudianteRepresentante> EstudiantesRepresentantes => Set<EstudianteRepresentante>();
        public DbSet<Horario> Horarios => Set<Horario>();
        public DbSet<Asistencia> Asistencias => Set<Asistencia>();
        public DbSet<Notificacion> Notificaciones => Set<Notificacion>();

<<<<<<< HEAD
        public DbSet<Profesor> Profesores => Set<Profesor>();
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<ProfesorCurso> ProfesorCursos => Set<ProfesorCurso>();


        protected override void OnModelCreating(ModelBuilder mb)
        {
            //mb.HasDefaultSchema("Main");
=======
        protected override void OnModelCreating(ModelBuilder mb)
        {
            //mb.HasDefaultSchema("Main");

>>>>>>> 5fe029b6863df2921e0af858ed485891b07eea34
            mb.Entity<Usuario>().ToTable("usuarios");
            mb.Entity<Estudiante>().ToTable("estudiantes");
            mb.Entity<Representante>().ToTable("representantes");
            mb.Entity<EstudianteRepresentante>().ToTable("estudiante_representante");
            mb.Entity<Horario>().ToTable("horarios");
            mb.Entity<Asistencia>().ToTable("asistencias");
            mb.Entity<Notificacion>().ToTable("notificaciones");
            mb.Entity<Profesor>().ToTable("profesores");
            mb.Entity<Curso>().ToTable("cursos");
            mb.Entity<ProfesorCurso>().ToTable("profesor_curso");
        }
<<<<<<< HEAD
    }
}
=======

    }
}
>>>>>>> 5fe029b6863df2921e0af858ed485891b07eea34
