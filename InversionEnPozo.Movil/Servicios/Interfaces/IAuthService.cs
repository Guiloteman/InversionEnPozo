using System;
using System.Collections.Generic;
using System.Text;

namespace InversionEnPozo.Movil.Servicios.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Exito, string Mensaje)> RegistrarAsync(string email, string password, string nombre, string rol);
        Task<(bool Exito, string Rol, string Mensaje)> LoginAsync(string email, string password);
    }
}
