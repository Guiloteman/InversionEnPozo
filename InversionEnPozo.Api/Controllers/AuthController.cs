using InversionEnPozo.Api.DTOs;
using InversionEnPozo.Api.Servicios;
using InversionEnPozo.Api.Servicios.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InversionEnPozo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] RegistroResponseDto model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var resultado = await _userManager.CreateAsync(user, model.Password);

            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            if (!await _roleManager.RoleExistsAsync(model.Rol))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Rol));
            }
            await _userManager.AddToRoleAsync(user, model.Rol);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var enlaceConfirmacion = $"https://empirical-pastel-galley.ngrok-free.dev/api/Auth/confirmar-email?email={user.Email}&token={Uri.EscapeDataString(token)}";

            string mensaje = $"<h3>¡Bienvenido a Inversión en Pozo!</h3><p>Por favor confirma tu cuenta haciendo clic en el siguiente enlace:</p><a href='{enlaceConfirmacion}'>Confirmar Correo</a>";

            await _emailService.EnviarEmailAsync(user.Email, "Confirma tu cuenta", mensaje);

            return Ok(new { mensaje = "Usuario registrado con éxito. Revisa tu correo para confirmar la cuenta." });
        }

        [HttpGet("confirmar-email")]
        public async Task<IActionResult> ConfirmarEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound("Usuario no encontrado.");

            var resultado = await _userManager.ConfirmEmailAsync(user, token);
            if (resultado.Succeeded)
                return Ok("¡Correo confirmado exitosamente! Ya puedes iniciar sesión desde la aplicación móvil.");

            return BadRequest("Error al confirmar el correo.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Credenciales inválidas.");

            var resultado = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!resultado.Succeeded)
                return Unauthorized("Contraseña incorrecta o usuario no autorizado.");

            if (!user.EmailConfirmed)
                return Unauthorized("Debes confirmar tu correo electrónico antes de iniciar sesión.");

            var roles = await _userManager.GetRolesAsync(user);
            var rolPrincipal = roles.FirstOrDefault() ?? "Inversionista";

            return Ok(new
            {
                mensaje = "Login exitoso",
                email = user.Email,
                rol = rolPrincipal
            });
        }

        [HttpPost("cambiar-password")]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordResponseDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("Usuario no encontrado.");

            var resultado = await _userManager.ChangePasswordAsync(user, model.PasswordActual, model.NuevoPassword);
            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            return Ok(new { mensaje = "Contraseña cambiada con éxito." });
        }
    }
}