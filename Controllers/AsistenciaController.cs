using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/asistencias")]
    public class AsistenciaController(AppDbContext db) : ControllerBase
    {
        // Filtros opcionales: estudianteId, fechaDesde, fechaHasta, estado.
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? estudianteId,
            [FromQuery] string? fechaDesde,
            [FromQuery] string? fechaHasta,
            [FromQuery] string? estado)
        {
            var q = db.Asistencias
                .Join(db.Estudiantes, a => a.EstudianteId, e => e.Id, (a, e) => new { a, e })
                .Join(db.Usuarios, x => x.e.UsuarioId, u => u.Id, (x, u) => new
                {
                    x.a.Id,
                    x.a.EstudianteId,
                    x.a.Fecha,
                    x.a.Hora,
                    x.a.Estado,
                    x.a.Origen,
                    x.a.Observacion,
                    x.a.CreatedAt,
                    Nombre = u.Nombre,
                    Codigo = x.e.Codigo
                })
                .AsQueryable();
            if (estudianteId.HasValue)
                q = q.Where(x => x.EstudianteId == estudianteId);
            if (fechaDesde != null)
                q = q.Where(x => x.Fecha.Date >= DateTime.Parse(fechaDesde).Date);
            if (fechaHasta != null)
                q = q.Where(x => x.Fecha.Date <= DateTime.Parse(fechaHasta).Date);
            if (estado != null)
                q = q.Where(x => x.Estado == estado.ToUpper());
            return Ok(await q.OrderByDescending(x => x.Fecha).ToListAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var a = await db.Asistencias.FindAsync(id);
            return a == null ? NotFound() : Ok(a);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Asistencia a)
        {
            a.Id = 0;

            // VERIFICAR SI YA EXISTE UNA ASISTENCIA PARA ESTE ESTUDIANTE EN ESTA FECHA
            var existe = await db.Asistencias
                .AnyAsync(x => x.EstudianteId == a.EstudianteId && x.Fecha.Date == a.Fecha.Date);

            if (existe)
            {
                return Conflict(new
                {
                    mensaje = "Ya existe un registro de asistencia para este estudiante en esta fecha"
                });
            }

            if (a.Estado == "PRESENTE" || a.Estado == "TARDANZA")
            {
                string dia = a.Fecha.DayOfWeek switch
                {
                    DayOfWeek.Monday => "LUNES",
                    DayOfWeek.Tuesday => "MARTES",
                    DayOfWeek.Wednesday => "MIERCOLES",
                    DayOfWeek.Thursday => "JUEVES",
                    DayOfWeek.Friday => "VIERNES",
                    DayOfWeek.Saturday => "SABADO",
                    _ => "DOMINGO"
                };
                var horario = await db.Horarios
                    .FirstOrDefaultAsync(h => h.DiaSemana == dia && h.Activo);
                if (horario != null)
                {
                    var limite = horario.HoraEntrada.Add(TimeSpan.FromMinutes(horario.ToleranciaMinutos));
                    a.Estado = a.Hora <= limite ? "PRESENTE" : "TARDANZA";
                }
            }

            db.Asistencias.Add(a);
            await db.SaveChangesAsync();
            return Ok(a);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Asistencia datos)
        {
            var a = await db.Asistencias.FindAsync(id);
            if (a == null) return NotFound();
            a.Estado = datos.Estado;
            a.Observacion = datos.Observacion;
            a.Origen = datos.Origen;
            await db.SaveChangesAsync();
            return Ok(a);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await db.Asistencias.FindAsync(id);
            if (a == null) return NotFound();
            db.Asistencias.Remove(a);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
