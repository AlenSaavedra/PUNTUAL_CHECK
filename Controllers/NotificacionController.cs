using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/notificaciones")]
    public class NotificacionController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? estudianteId,
            [FromQuery] string? estadoEnvio,
            [FromQuery] string? tipo)
        {
            var q = db.Notificaciones
                .Join(db.Estudiantes, n => n.EstudianteId, e => e.Id, (n, e) => new { n, e })
                .Join(db.Usuarios, x => x.e.UsuarioId, u => u.Id, (x, u) => new { x.n, NombreEstudiante = u.Nombre })
                .Join(db.Representantes, x => x.n.RepresentanteId, r => r.Id, (x, r) => new { x.n, x.NombreEstudiante, r })
                .Join(db.Usuarios, x => x.r.UsuarioId, u => u.Id, (x, u) => new
                {
                    x.n.Id,
                    x.n.EstudianteId,
                    x.n.RepresentanteId,
                    x.n.FechaEvento,
                    x.n.Tipo,
                    x.n.Canal,
                    x.n.EstadoEnvio,
                    x.n.Detalle,
                    x.n.CreatedAt,
                    x.NombreEstudiante,
                    NombreRepresentante = u.Nombre
                })
                .AsQueryable();

            if (estudianteId.HasValue) q = q.Where(x => x.EstudianteId == estudianteId);
            if (estadoEnvio != null) q = q.Where(x => x.EstadoEnvio == estadoEnvio.ToUpper());
            if (tipo != null) q = q.Where(x => x.Tipo == tipo.ToUpper());

            return Ok(await q.OrderByDescending(x => x.CreatedAt).ToListAsync());
        }

        [HttpGet("por-representante/{representanteId:int}")]
        public async Task<IActionResult> GetByRepresentante(
            int representanteId,
            [FromQuery] string? estadoEnvio,
            [FromQuery] string? tipo)
        {
            var q = db.Notificaciones
                .Where(n => n.RepresentanteId == representanteId)
                .Join(db.Estudiantes, n => n.EstudianteId, e => e.Id, (n, e) => new { n, e })
                .Join(db.Usuarios, x => x.e.UsuarioId, u => u.Id, (x, u) => new
                {
                    x.n.Id,
                    x.n.EstudianteId,
                    x.n.RepresentanteId,
                    x.n.FechaEvento,
                    x.n.Tipo,
                    x.n.Canal,
                    x.n.EstadoEnvio,
                    x.n.Detalle,
                    x.n.CreatedAt,
                    NombreEstudiante = u.Nombre,
                    CodigoEstudiante = x.e.Codigo
                })
                .AsQueryable();

            // Filtros opcionales
            if (estadoEnvio != null) q = q.Where(x => x.EstadoEnvio == estadoEnvio.ToUpper());
            if (tipo != null) q = q.Where(x => x.Tipo == tipo.ToUpper());

            var resultado = await q.OrderByDescending(x => x.CreatedAt).ToListAsync();

            if (!resultado.Any())
                return NotFound(new { mensaje = "No se encontraron notificaciones para este representante" });

            return Ok(resultado);
        }
        

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var n = await db.Notificaciones.FindAsync(id);
            return n == null ? NotFound() : Ok(n);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notificacion n)
        {
            n.Id = 0;
            n.EstadoEnvio = "PENDIENTE";
            db.Notificaciones.Add(n);
            await db.SaveChangesAsync();
            return Ok(n);
        }

        // Cambiar estado de envío
        [HttpPatch("{id:int}/estado-envio/{nuevoEstado}")]
        public async Task<IActionResult> UpdateEstado(int id, string nuevoEstado)
        {
            var n = await db.Notificaciones.FindAsync(id);
            if (n == null) return NotFound();

            n.EstadoEnvio = nuevoEstado.ToUpper();
            await db.SaveChangesAsync();
            return Ok(new { n.Id, n.EstadoEnvio });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var n = await db.Notificaciones.FindAsync(id);
            if (n == null) return NotFound();
            db.Notificaciones.Remove(n);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
