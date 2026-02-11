using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/representantes")]
    public class RepresentanteController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await db.Representantes
                .Join(db.Usuarios, r => r.UsuarioId, u => u.Id, (r, u) => new
                {
                    r.Id,
                    r.UsuarioId,
                    r.Telefono,
                    r.Activo,
                    r.CreatedAt,
                    Nombre = u.Nombre
                })
                .ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await db.Representantes
                .Join(db.Usuarios, r => r.UsuarioId, u => u.Id, (r, u) => new
                {
                    r.Id,
                    r.UsuarioId,
                    r.Telefono,
                    r.Activo,
                    Nombre = u.Nombre
                })
                .FirstOrDefaultAsync(x => x.Id == id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Representante r)
        {
            r.Id = 0;
            db.Representantes.Add(r);
            await db.SaveChangesAsync();
            return Ok(r);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Representante datos)
        {
            var r = await db.Representantes.FindAsync(id);
            if (r == null) return NotFound();

            r.Telefono = datos.Telefono;
            r.Activo = datos.Activo;

            await db.SaveChangesAsync();
            return Ok(r);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var r = await db.Representantes.FindAsync(id);
            if (r == null) return NotFound();
            db.Representantes.Remove(r);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
