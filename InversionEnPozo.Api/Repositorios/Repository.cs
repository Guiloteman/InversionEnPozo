using InversionEnPozo.Api.Datos;
using InversionEnPozo.Api.Datos.Interfaces;
using InversionEnPozo.Api.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InversionEnPozo.Api.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IAppDbContext _context;

        public Repository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync() => await _context.Set<T>().ToListAsync();

        public async Task<T?> ObtenerPorIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado) =>
            await _context.Set<T>().Where(predicado).ToListAsync();

        public async Task AgregarAsync(T entidad) => await _context.Set<T>().AddAsync(entidad);

        public void Actualizar(T entidad) => _context.Set<T>().Update(entidad);

        public void Eliminar(T entidad) => _context.Set<T>().Remove(entidad);
    }
}