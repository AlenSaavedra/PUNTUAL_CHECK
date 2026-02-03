namespace API_PUNTUALCHECK.Models
{

    public class General 
    {
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? PasswordHash { get; set; }
        public string? Rol { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Estudiante
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? Codigo { get; set; }
        public string? QrToken { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Representante
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class EstudianteRepresentante
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int RepresentanteId { get; set; }
        public string Parentesco { get; set; } = "OTRO";
    }

    public class Horario
    {
        public int Id { get; set; }
        public string? DiaSemana { get; set; }
        public TimeOnly HoraEntrada { get; set; }
        public int ToleranciaMinutos { get; set; } = 10;
        public bool Activo { get; set; } = true;
    }

    public class Asistencia
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string? Estado { get; set; }
        public string Origen { get; set; } = "MOVIL";
        public string? Observacion { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Notificacion
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int RepresentanteId { get; set; }
        public DateOnly FechaEvento { get; set; }
        public string? Tipo { get; set; }
        public string Canal { get; set; } = "EMAIL";
        public string EstadoEnvio { get; set; } = "PENDIENTE";
        public string? Detalle { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

