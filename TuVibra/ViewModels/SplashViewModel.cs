using TuVibra.Services;

namespace TuVibra.ViewModels;


/// Lógica y estado de la pantalla de bienvenida: progreso de carga,
/// mensaje actual y navegación a la siguiente pantalla.
/// No conoce controles ni animaciones: eso es trabajo de la vista.

public class SplashViewModel : ViewModelBase
{
    // Constantes de configuración: se cambian aquí sin tocar la lógica.
    private const int TotalMs = 3000;           // Duración total de la espera
    private const int TickMs = 50;              // Cada cuánto se actualiza el progreso
    private const string NextRoute = "//MainPage";

    private static readonly string[] LoadingMessages =
    {
        "Cargando tu música...",
        "Afinando los instrumentos...",
        "Todo listo"
    };

    private readonly INavigationService _navigation;

    private double _progress;
    private string _statusMessage = LoadingMessages[0];

    // Se inyecta la interfaz, no la clase concreta (DIP).
    public SplashViewModel(INavigationService navigation)
    {
        _navigation = navigation;
    }

    ///Progreso de 0 a 1, enlazado a la ProgressBar.
    public double Progress
    {
        get => _progress;
        private set => SetProperty(ref _progress, value);
    }

    ///Mensaje que se muestra mientras carga, enlazado al Label.
    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    /// Simula la carga: avanza el progreso en pequeños pasos y cambia el
    /// mensaje según el avance. Es el "uso del tiempo de espera en pantalla".
   
    public async Task RunLoadingAsync()
    {
        int elapsed = 0;

        while (elapsed < TotalMs)
        {
            await Task.Delay(TickMs);
            elapsed += TickMs;

            Progress = Math.Min(1.0, elapsed / (double)TotalMs);

            // Elige el mensaje según el porcentaje: 0-33 %, 33-66 %, 66-100 %.
            int index = Math.Min((int)(Progress * LoadingMessages.Length),
                                 LoadingMessages.Length - 1);
            StatusMessage = LoadingMessages[index];
        }
    }

    ///Navega a la siguiente pantalla.
    public Task GoToNextAsync() => _navigation.GoToAsync(NextRoute);
}