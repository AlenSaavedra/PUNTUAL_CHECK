using API_PUNTUALCHECK.Models;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController(AppDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await db.Usuarios.ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var u = await db.Usuarios.FindAsync(id);
            return u == null ? NotFound() : Ok(u);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario u)
        {
            u.Id = 0;
            db.Usuarios.Add(u);
            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario datos)
        {
            var u = await db.Usuarios.FindAsync(id);
            if (u == null) return NotFound();

            u.Nombre = datos.Nombre;
            u.Correo = datos.Correo;
            u.Rol = datos.Rol;
            u.Activo = datos.Activo;
            if (datos.PasswordHash != null) u.PasswordHash = datos.PasswordHash;

            await db.SaveChangesAsync();
            return Ok(u);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var u = await db.Usuarios.FindAsync(id);
            if (u == null) return NotFound();
            db.Usuarios.Remove(u);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
