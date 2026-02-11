// Controllers/ProfesorController.cs
using API_PUNTUALCHECK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using API_PUNTUALCHECK.Utils;
=======
>>>>>>> 5fe029b6863df2921e0af858ed485891b07eea34

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/profesores")]
    public class ProfesorController(AppDbContext db) : ControllerBase
    {
        // GET: api/profesores
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? activo)
        {
            var q = db.Profesores
                .Join(db.Usuarios, p => p.UsuarioId, u => u.Id, (p, u) => new
                {
                    p.Id,
                    p.UsuarioId,
                    p.Especialidad,
                    p.Activo,
                    p.CreatedAt,
                    Nombre = u.Nombre,
                    Correo = u.Correo
                })
                .AsQueryable();

            if (activo.HasValue)
                q = q.Where(x => x.Activo == activo);

            return Ok(await q.OrderBy(x => x.Nombre).ToListAsync());
        }

        // GET: api/profesores/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesor = await db.Profesores
                .Where(p => p.Id == id)
                .Join(db.Usuarios, p => p.UsuarioId, u => u.Id, (p, u) => new
                {
                    p.Id,
                    p.UsuarioId,
                    p.Especialidad,
                    p.Activo,
                    p.CreatedAt,
                    Nombre = u.Nombre,
                    Correo = u.Correo
                })
                .FirstOrDefaultAsync();

            return profesor == null ? NotFound() : Ok(profesor);
        }

        // GET: api/profesores/{id}/cursos
        [HttpGet("{id:int}/cursos")]
        public async Task<IActionResult> GetCursos(int id)
        {
            var cursos = await db.ProfesorCursos
                .Where(pc => pc.ProfesorId == id && pc.Activo)
                .Join(db.Cursos, pc => pc.CursoId, c => c.Id, (pc, c) => new
                {
                    c.Id,
                    c.Nombre,
                    c.Codigo,
                    c.Descripcion,
                    pc.AsignadoDesde,
                    RelacionId = pc.Id
                })
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            return Ok(cursos);
        }

        // POST: api/profesores
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Profesor p)
        {
            p.Id = 0;

            // Verificar que el usuario existe
            var usuario = await db.Usuarios.FindAsync(p.UsuarioId);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            // Verificar que no exista ya un profesor con este usuario
            var existe = await db.Profesores.AnyAsync(x => x.UsuarioId == p.UsuarioId);
            if (existe)
                return Conflict(new { mensaje = "Ya existe un profesor asociado a este usuario" });

            db.Profesores.Add(p);
            await db.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Profesor creado exitosamente",
                profesor = new
                {
                    p.Id,
                    p.UsuarioId,
                    p.Especialidad,
                    p.Activo,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo
                }
            });
        }

        // PUT: api/profesores/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Profesor datos)
        {
            var p = await db.Profesores.FindAsync(id);
            if (p == null) return NotFound();

            p.Especialidad = datos.Especialidad;
            p.Activo = datos.Activo;

            await db.SaveChangesAsync();
            return Ok(p);
        }

        // DELETE: api/profesores/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await db.Profesores.FindAsync(id);
            if (p == null) return NotFound();

            db.Profesores.Remove(p);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
