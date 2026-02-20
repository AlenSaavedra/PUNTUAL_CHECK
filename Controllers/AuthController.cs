using API_PUNTUALCHECK.DTOs;
using API_PUNTUALCHECK.Utils;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = API_PUNTUALCHECK.DTOs.LoginRequest;

namespace API_PUNTUALCHECK.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(AppDbContext db, IConfiguration config) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 1. Buscar usuario por correo
            var usuario = await db.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Activo);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            // 2. Validar password (texto plano por ahora)
            if (usuario.PasswordHash != request.Password)
                return Unauthorized(new { mensaje = "Credenciales inválidas" });

            // 3. Generar token JWT
            var token = GenerarToken(usuario.Id, usuario.Nombre!, usuario.Rol!);

            return Ok(new LoginResponse
            {
                Token = token,
                Nombre = usuario.Nombre!,
                Rol = usuario.Rol!,
                UsuarioId = usuario.Id
            });
        }

        private string GenerarToken(int id, string nombre, string rol)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expira = DateTime.UtcNow.AddMinutes(
                double.Parse(config["Jwt:ExpiresInMinutes"]!));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, nombre),
                new Claim(ClaimTypes.Role, rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: expira,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}