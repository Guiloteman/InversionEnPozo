using InversionEnPozo.Api.Datos;
using InversionEnPozo.Api.Datos.Interfaces;
using InversionEnPozo.Api.Modelos;
using InversionEnPozo.Api.Repositorios;
using InversionEnPozo.Api.Repositorios.Interfaces;
using InversionEnPozo.Api.Servicios.Interfaces;

namespace InversionEnPozo.Api.Servicios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppDbContext _context;
        public IRepository<ProyectoInversion> Proyectos { get; private set; }
        public IRepository<Inversion> Inversiones { get; private set; }
        public IRepository<HitoObra> Hitos { get; private set; }

        public UnitOfWork(IAppDbContext context)
        {
            _context = context;
            Proyectos = new Repository<ProyectoInversion>(_context);
            Inversiones = new Repository<Inversion>(_context);
            Hitos = new Repository<HitoObra>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}