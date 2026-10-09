namespace TuVibra.Services;

/// Contrato de navegación admite parámetros opcionales para enviar
/// datos (por ejemplo, la canción elegida) a la pantalla de destino.

public interface INavigationService
{
    Task GoToAsync(string route, IDictionary<string, object>? parameters = null);
}