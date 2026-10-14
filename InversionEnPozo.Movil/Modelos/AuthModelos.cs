namespace InversionEnPozo.Movil.Modelos
{
    public class LoginResponseDto
    {
        public string Mensaje { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
    public class RegistroRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Rol { get; set; } = "Inversionista";
    }

    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ConfirmarEmailRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    public class CambiarPasswordRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordActual { get; set; } = string.Empty;
        public string NuevoPassword { get; set; } = string.Empty;
    }
}