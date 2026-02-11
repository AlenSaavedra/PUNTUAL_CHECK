using API_PUNTUALCHECK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/profesor-curso")]
    public class ProfesorCursoController(AppDbContext db) : ControllerBase
    {
        // POST: api/profesor-curso (Asignar curso a profesor)
        [HttpPost]
        public async Task<IActionResult> AsignarCurso([FromBody] ProfesorCurso pc)
        {
            pc.Id = 0;

            // Verificar que el profesor existe
            var profesor = await db.Profesores.FindAsync(pc.ProfesorId);
            if (profesor == null)
                return NotFound(new { mensaje = "Profesor no encontrado" });

            // Verificar que el curso existe
            var curso = await db.Cursos.FindAsync(pc.CursoId);
            if (curso == null)
                return NotFound(new { mensaje = "Curso no encontrado" });

            // Verificar que no exista ya esta asignación activa
            var existe = await db.ProfesorCursos
                .AnyAsync(x => x.ProfesorId == pc.ProfesorId 
                            && x.CursoId == pc.CursoId 
                            && x.Activo);

            if (existe)
                return Conflict(new { mensaje = "Este curso ya está asignado a este profesor" });

            db.ProfesorCursos.Add(pc);
            await db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Curso asignado exitosamente",
                asignacion = pc
            });
        }

        // DELETE: api/profesor-curso/{id} (Desasignar curso)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DesasignarCurso(int id)
        {
            var pc = await db.ProfesorCursos.FindAsync(id);
            if (pc == null) return NotFound();

            db.ProfesorCursos.Remove(pc);
            await db.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/profesor-curso/{id} (Activar/Desactivar asignación)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProfesorCurso datos)
        {
            var pc = await db.ProfesorCursos.FindAsync(id);
            if (pc == null) return NotFound();

            pc.Activo = datos.Activo;
            await db.SaveChangesAsync();
            return Ok(pc);
        }
    }
}
