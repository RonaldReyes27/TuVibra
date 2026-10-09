using TuVibra.Models;

namespace TuVibra.Services;


/// Contrato para buscar música. El ViewModel depende de esta interfaz,
/// no de iTunes: si mañana cambias de API, solo creas otra implementación
/// (principios abierto/cerrado e inversión de dependencias).
public interface IMusicService
{
    Task<IReadOnlyList<Song>> SearchAsync(string term, CancellationToken cancellationToken = default);
}