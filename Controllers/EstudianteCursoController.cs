using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/estudiante-curso")]
    public class EstudianteCursoController(AppDbContext db) : ControllerBase
    {
        // GET: api/estudiante-curso/estudiante/{estudianteId}
        // Cursos en los que está inscrito un estudiante
        [HttpGet("estudiante/{estudianteId:int}")]
        public async Task<IActionResult> GetCursosPorEstudiante(int estudianteId)
        {
            var existe = await db.Estudiantes.AnyAsync(e => e.Id == estudianteId);
            if (!existe) return NotFound(new { mensaje = "Estudiante no encontrado" });

            var cursos = await db.EstudianteCursos
                .Where(ec => ec.EstudianteId == estudianteId && ec.Activo)
                .Join(db.Cursos, ec => ec.CursoId, c => c.Id, (ec, c) => new
                {
                    RelacionId       = ec.Id,
                    ec.EstudianteId,
                    Curso = new
                    {
                        c.Id,
                        c.Nombre,
                        c.Codigo,
                        c.Descripcion
                    },
                    ec.FechaInscripcion
                })
                .ToListAsync();

            return Ok(cursos);
        }

        // GET: api/estudiante-curso/curso/{cursoId}
        // Estudiantes inscritos en un curso
        [HttpGet("curso/{cursoId:int}")]
        public async Task<IActionResult> GetEstudiantesPorCurso(int cursoId)
        {
            var existe = await db.Cursos.AnyAsync(c => c.Id == cursoId);
            if (!existe) return NotFound(new { mensaje = "Curso no encontrado" });

            var estudiantes = await db.EstudianteCursos
                .Where(ec => ec.CursoId == cursoId && ec.Activo)
                .Join(db.Estudiantes, ec => ec.EstudianteId, e => e.Id, (ec, e) => new { ec, e })
                .Join(db.Usuarios, x => x.e.UsuarioId, u => u.Id, (x, u) => new
                {
                    RelacionId      = x.ec.Id,
                    x.ec.CursoId,
                    Estudiante = new
                    {
                        x.e.Id,
                        x.e.Codigo,
                        x.e.QrToken,
                        Nombre = u.Nombre,
                        Correo = u.Correo
                    },
                    x.ec.FechaInscripcion
                })
                .ToListAsync();

            return Ok(estudiantes);
        }

        // POST: api/estudiante-curso
        // Inscribir un estudiante a un curso
        [HttpPost]
        public async Task<IActionResult> Inscribir([FromBody] EstudianteCurso request)
        {
            var estudianteExiste = await db.Estudiantes.AnyAsync(e => e.Id == request.EstudianteId);
            if (!estudianteExiste)
                return NotFound(new { mensaje = "Estudiante no encontrado" });

            var cursoExiste = await db.Cursos.AnyAsync(c => c.Id == request.CursoId);
            if (!cursoExiste)
                return NotFound(new { mensaje = "Curso no encontrado" });

            var yaInscrito = await db.EstudianteCursos.AnyAsync(ec =>
                ec.EstudianteId == request.EstudianteId &&
                ec.CursoId == request.CursoId &&
                ec.Activo);

            if (yaInscrito)
                return Conflict(new { mensaje = "El estudiante ya está inscrito en este curso" });

            request.Id = 0;
            request.FechaInscripcion = DateTime.UtcNow;
            request.Activo = true;

            db.EstudianteCursos.Add(request);
            await db.SaveChangesAsync();

            return Ok(new { mensaje = "Inscripción exitosa", inscripcion = request });
        }

        // DELETE: api/estudiante-curso/{id}
        // Desinscribir (desactiva la relación)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Desinscribir(int id)
        {
            var ec = await db.EstudianteCursos.FindAsync(id);
            if (ec == null) return NotFound();

            ec.Activo = false;
            await db.SaveChangesAsync();

            return Ok(new { mensaje = "Estudiante desinscrito del curso" });
        }
    }
}
