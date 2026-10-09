
using System.ComponentModel;
using TuVibra.ViewModels;

namespace TuVibra;

/// <summary>
/// Vista del reproductor. Se encarga de lo visual: manejar el control de audio,
/// animar el disco y reaccionar a los cambios de estado del ViewModel.
/// </summary>
public partial class PlayerPage : ContentPage
{
    private const uint EntranceMs = 600;    // Entrada del disco
    private const uint SpinMs = 8000;       // Una vuelta completa del disco

    private readonly PlayerViewModel _viewModel;
    private bool _isSpinning;

    public PlayerPage(PlayerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // La vista reacciona a IsPlaying (estado del ViewModel).
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;

        // Eventos del reproductor: se los pasamos al ViewModel.
        MediaPlayer.PositionChanged += OnPositionChanged;
        MediaPlayer.MediaEnded += OnMediaEnded;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Transición de entrada: el disco aparece y crece.
        await Task.WhenAll(
            DiscArea.FadeTo(1, EntranceMs, Easing.CubicOut),
            DiscArea.ScaleTo(1, EntranceMs, Easing.SpringOut));

        // Cargamos el preview y arrancamos la reproducción.
        if (!string.IsNullOrEmpty(_viewModel.PreviewUrl))
        {
            MediaPlayer.Source = _viewModel.PreviewUrl;
            _viewModel.StartPlayback();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Al salir de la pantalla, la música debe parar.
        _viewModel.StopPlayback();
        MediaPlayer.Stop();
    }

    // Cuando IsPlaying cambia, pausamos/reanudamos el audio y el disco.
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(PlayerViewModel.IsPlaying))
            return;

        if (_viewModel.IsPlaying)
        {
            MediaPlayer.Play();
            StartSpin();
        }
        else
        {
            MediaPlayer.Pause();
            StopSpin();
        }
    }

    private void OnPositionChanged(object? sender, EventArgs e)
        => _viewModel.UpdatePosition(MediaPlayer.Position, MediaPlayer.Duration);

    private async void OnMediaEnded(object? sender, EventArgs e)
    {
        _viewModel.OnPlaybackEnded();
        await MediaPlayer.SeekTo(TimeSpan.Zero);   // Listo para reproducir de nuevo
    }

    // Animación del botón al tocarlo: se achica y vuelve.
    private async void OnPlayButtonClicked(object? sender, EventArgs e)
    {
        await PlayButton.ScaleTo(0.92, 80);
        await PlayButton.ScaleTo(1, 80, Easing.SpringOut);
    }

    // Disco girando: vueltas continuas hasta que se pause.
    private async void StartSpin()
    {
        if (_isSpinning)
            return;

        _isSpinning = true;
        while (_isSpinning)
        {
            Disc.Rotation %= 360;
            await Disc.RotateTo(Disc.Rotation + 360, SpinMs, Easing.Linear);
        }
    }

    private void StopSpin()
    {
        _isSpinning = false;
        Disc.CancelAnimations();   // Se queda en el ángulo actual
    }
}