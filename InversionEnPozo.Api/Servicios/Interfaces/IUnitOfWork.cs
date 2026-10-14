using InversionEnPozo.Api.Modelos;
using InversionEnPozo.Api.Repositorios;
using InversionEnPozo.Api.Repositorios.Interfaces;

namespace InversionEnPozo.Api.Servicios.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ProyectoInversion> Proyectos { get; }
        IRepository<Inversion> Inversiones { get; }
        IRepository<HitoObra> Hitos { get; }
        Task<int> CompleteAsync();
    }
}