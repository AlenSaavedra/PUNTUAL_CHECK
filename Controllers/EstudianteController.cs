using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/estudiantes")]
    public class EstudianteController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await db.Estudiantes
                .Join(db.Usuarios, e => e.UsuarioId, u => u.Id, (e, u) => new
                {
                    e.Id,
                    e.UsuarioId,
                    e.Codigo,
                    e.QrToken,
                    e.Activo,
                    e.CreatedAt,
                    Nombre = u.Nombre
                })
                .ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var e = await db.Estudiantes
                .Join(db.Usuarios, e => e.UsuarioId, u => u.Id, (e, u) => new
                {
                    e.Id,
                    e.UsuarioId,
                    e.Codigo,
                    e.QrToken,
                    e.Activo,
                    Nombre = u.Nombre
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            return e == null ? NotFound() : Ok(e);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Estudiante e)
        {
            e.Id = 0;
            e.QrToken = Guid.NewGuid().ToString();
            db.Estudiantes.Add(e);
            await db.SaveChangesAsync();
            return Ok(e);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Estudiante datos)
        {
            var e = await db.Estudiantes.FindAsync(id);
            if (e == null) return NotFound();

            e.Codigo = datos.Codigo;
            e.Activo = datos.Activo;

            await db.SaveChangesAsync();
            return Ok(e);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var e = await db.Estudiantes.FindAsync(id);
            if (e == null) return NotFound();
            db.Estudiantes.Remove(e);
            await db.SaveChangesAsync();
            return NoContent();
        }

        // Regenera el QR token
        [HttpPatch("{id:int}/regenerar-qr")]
        public async Task<IActionResult> RegenerarQr(int id)
        {
            var e = await db.Estudiantes.FindAsync(id);
            if (e == null) return NotFound();
            e.QrToken = Guid.NewGuid().ToString();
            await db.SaveChangesAsync();
            return Ok(new { e.Id, e.QrToken });
        }
    }
}
