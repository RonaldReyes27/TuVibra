using TuVibra.ViewModels;

namespace TuVibra;

/// <summary>
/// Vista del splash. En MVVM el code-behind solo se encarga de lo visual:
/// conectar el ViewModel y reproducir animaciones. La lógica vive en SplashViewModel.
/// </summary>
public partial class SplashPage : ContentPage
{
    private const uint EntranceMs = 800;  // Duración de la animación de entrada
    private const uint ExitMs = 400;      // Duración del fade de salida

    private readonly SplashViewModel _viewModel;

    // El ViewModel llega por inyección de dependencias (registrado en MauiProgram).
    public SplashPage(SplashViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;   // Conecta la vista con su ViewModel
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await PlayEntranceAnimationAsync();   // Vista: animación de entrada
        await _viewModel.RunLoadingAsync();   // ViewModel: lógica de carga
        await this.FadeTo(0, ExitMs);         // Vista: transición de salida
        await _viewModel.GoToNextAsync();     // ViewModel: navegación
    }

    private async Task PlayEntranceAnimationAsync()
    {
        // Logo: fade y escala al mismo tiempo.
        await Task.WhenAll(
            LogoCard.FadeTo(1, EntranceMs, Easing.CubicOut),
            LogoCard.ScaleTo(1, EntranceMs, Easing.SpringOut));

        // Luego aparecen la barra y el mensaje.
        await Task.WhenAll(
            LoadingBar.FadeTo(1, 300),
            StatusLabel.FadeTo(1, 300));
    }
}