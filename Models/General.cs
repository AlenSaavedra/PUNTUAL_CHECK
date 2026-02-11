using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_PUNTUALCHECK.Models
{
    public class General
    {
    }

    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        public string? Nombre { get; set; }

        [Column("correo")]
        public string? Correo { get; set; }

        [Column("password_hash")]
        public string? PasswordHash { get; set; }

        [Column("rol")]
        public string? Rol { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("estudiantes")]
    public class Estudiante
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("codigo")]
        public string? Codigo { get; set; }

        [Column("qr_token")]
        public string? QrToken { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("representantes")]
    public class Representante
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("estudiante_representante")]
    public class EstudianteRepresentante
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("estudiante_id")]
        public int EstudianteId { get; set; }

        [Column("representante_id")]
        public int RepresentanteId { get; set; }

        [Column("parentesco")]
        public string Parentesco { get; set; } = "OTRO";
    }

    [Table("horarios")]
    public class Horario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("dia_semana")]
        public string? DiaSemana { get; set; }

        [Column("hora_entrada")]
        public TimeSpan HoraEntrada { get; set; }


        [Column("tolerancia_minutos")]
        public int ToleranciaMinutos { get; set; } = 10;

        [Column("activo")]
        public bool Activo { get; set; } = true;
    }

    [Table("asistencias")]
    public class Asistencia
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("estudiante_id")]
        public int EstudianteId { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }
        
        [Column("hora")]
        public TimeSpan Hora { get; set; }


        [Column("estado")]
        public string? Estado { get; set; }

        [Column("origen")]
        public string Origen { get; set; } = "MOVIL";

        [Column("observacion")]
        public string? Observacion { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("notificaciones")]
    public class Notificacion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("estudiante_id")]
        public int EstudianteId { get; set; }

        [Column("representante_id")]
        public int RepresentanteId { get; set; }

        [Column("fecha_evento")]
        public DateTime FechaEvento { get; set; }


        [Column("tipo")]
        public string? Tipo { get; set; }

        [Column("canal")]
        public string Canal { get; set; } = "EMAIL";

        [Column("estado_envio")]
        public string EstadoEnvio { get; set; } = "PENDIENTE";

        [Column("detalle")]
        public string? Detalle { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    [Table("profesores")]
    public class Profesor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
    
        [Column("especialidad")]
        public string? Especialidad { get; set; }
    
        [Column("activo")]
        public bool Activo { get; set; } = true;
    
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
    [Table("cursos")]
    public class Curso
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;
    
        [Column("codigo")]
        public string? Codigo { get; set; }
    
        [Column("descripcion")]
        public string? Descripcion { get; set; }
    
        [Column("activo")]
        public bool Activo { get; set; } = true;
    
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
    [Table("profesor_curso")]
    public class ProfesorCurso
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    
        [Column("profesor_id")]
        public int ProfesorId { get; set; }
    
        [Column("curso_id")]
        public int CursoId { get; set; }
    
        [Column("asignado_desde")]
        public DateTime AsignadoDesde { get; set; } = DateTime.UtcNow;
    
        [Column("activo")]
        public bool Activo { get; set; } = true;
    }

    
}
