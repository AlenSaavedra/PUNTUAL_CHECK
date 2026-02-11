// Controllers/CursoController.cs
using API_PUNTUALCHECK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_PUNTUALCHECK.Utils;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/cursos")]
    public class CursoController(AppDbContext db) : ControllerBase
    {
        // GET: api/cursos
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? activo)
        {
            var q = db.Cursos.AsQueryable();

            if (activo.HasValue)
                q = q.Where(x => x.Activo == activo);

            return Ok(await q.OrderBy(x => x.Nombre).ToListAsync());
        }

        // GET: api/cursos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var c = await db.Cursos.FindAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        // GET: api/cursos/{id}/profesores
        [HttpGet("{id:int}/profesores")]
        public async Task<IActionResult> GetProfesores(int id)
        {
            var profesores = await db.ProfesorCursos
                .Where(pc => pc.CursoId == id && pc.Activo)
                .Join(db.Profesores, pc => pc.ProfesorId, p => p.Id, (pc, p) => new { pc, p })
                .Join(db.Usuarios, x => x.p.UsuarioId, u => u.Id, (x, u) => new
                {
                    x.p.Id,
                    Nombre = u.Nombre,
                    Correo = u.Correo,
                    x.p.Especialidad,
                    x.pc.AsignadoDesde,
                    RelacionId = x.pc.Id
                })
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            return Ok(profesores);
        }

        // POST: api/cursos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Curso c)
        {
            c.Id = 0;

            // Verificar que no exista un curso con el mismo código
            if (!string.IsNullOrEmpty(c.Codigo))
            {
                var existe = await db.Cursos.AnyAsync(x => x.Codigo == c.Codigo);
                if (existe)
                    return Conflict(new { mensaje = "Ya existe un curso con este código" });
            }

            db.Cursos.Add(c);
            await db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Curso creado exitosamente",
                curso = c
            });
        }

        // PUT: api/cursos/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Curso datos)
        {
            var c = await db.Cursos.FindAsync(id);
            if (c == null) return NotFound();

            c.Nombre = datos.Nombre;
            c.Codigo = datos.Codigo;
            c.Descripcion = datos.Descripcion;
            c.Activo = datos.Activo;

            await db.SaveChangesAsync();
            return Ok(c);
        }

        // DELETE: api/cursos/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await db.Cursos.FindAsync(id);
            if (c == null) return NotFound();

            db.Cursos.Remove(c);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
