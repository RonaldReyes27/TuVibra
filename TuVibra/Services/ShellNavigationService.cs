namespace TuVibra.Services;

/// Implementación de la navegación con Shell de MAUI.
 
public class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
        => parameters is null
            ? Shell.Current.GoToAsync(route)
            : Shell.Current.GoToAsync(route, parameters);
}