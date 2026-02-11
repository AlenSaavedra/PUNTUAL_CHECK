using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/horarios")]
    public class HorarioController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await db.Horarios.ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var h = await db.Horarios.FindAsync(id);
            return h == null ? NotFound() : Ok(h);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Horario h)
        {
            h.Id = 0;
            db.Horarios.Add(h);
            await db.SaveChangesAsync();
            return Ok(h);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Horario datos)
        {
            var h = await db.Horarios.FindAsync(id);
            if (h == null) return NotFound();

            h.DiaSemana = datos.DiaSemana;
            h.HoraEntrada = datos.HoraEntrada;
            h.ToleranciaMinutos = datos.ToleranciaMinutos;
            h.Activo = datos.Activo;

            await db.SaveChangesAsync();
            return Ok(h);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var h = await db.Horarios.FindAsync(id);
            if (h == null) return NotFound();
            db.Horarios.Remove(h);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
