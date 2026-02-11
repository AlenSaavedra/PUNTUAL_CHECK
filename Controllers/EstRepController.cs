using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/estudiante-representante")]
    public class EstRepController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await db.EstudiantesRepresentantes
                .Join(db.Estudiantes, er => er.EstudianteId, e => e.Id, (er, e) => new { er, e })
                .Join(db.Usuarios, x => x.e.UsuarioId, u => u.Id, (x, u) => new { x.er, NombreEstudiante = u.Nombre })
                .Join(db.Representantes, x => x.er.RepresentanteId, r => r.Id, (x, r) => new { x.er, x.NombreEstudiante, r })
                .Join(db.Usuarios, x => x.r.UsuarioId, u => u.Id, (x, u) => new
                {
                    x.er.Id,
                    x.er.EstudianteId,
                    x.er.RepresentanteId,
                    x.er.Parentesco,
                    x.NombreEstudiante,
                    NombreRepresentante = u.Nombre
                })
                .ToListAsync());

        // Representantes de un estudiante específico
        [HttpGet("por-estudiante/{estudianteId:int}")]
        public async Task<IActionResult> GetByEstudiante(int estudianteId) =>
            Ok(await db.EstudiantesRepresentantes
                .Where(er => er.EstudianteId == estudianteId)
                .Join(db.Representantes, er => er.RepresentanteId, r => r.Id, (er, r) => new { er, r })
                .Join(db.Usuarios, x => x.r.UsuarioId, u => u.Id, (x, u) => new
                {
                    x.er.Id,
                    x.er.RepresentanteId,
                    x.er.Parentesco,
                    Nombre = u.Nombre
                })
                .ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EstudianteRepresentante er)
        {
            er.Id = 0;
            db.EstudiantesRepresentantes.Add(er);
            await db.SaveChangesAsync();
            return Ok(er);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var er = await db.EstudiantesRepresentantes.FindAsync(id);
            if (er == null) return NotFound();
            db.EstudiantesRepresentantes.Remove(er);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
