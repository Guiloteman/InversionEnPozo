using Microsoft.EntityFrameworkCore;

namespace InversionEnPozo.Api.Datos.Interfaces
{
    public interface IAppDbContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}