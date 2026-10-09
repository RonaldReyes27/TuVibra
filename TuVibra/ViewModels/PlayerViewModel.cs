using System.Windows.Input;
using TuVibra.Models;
using TuVibra.Services;

namespace TuVibra.ViewModels;


/// Estado y lógica del reproductor: datos de la canción, si está sonando,
/// progreso y textos de tiempo. El ViewModel es la "fuente de la verdad" de
/// IsPlaying; la vista reacciona a ese valor para pausar/reanudar el audio
/// y la animación del disco.
public class PlayerViewModel : ViewModelBase, IQueryAttributable
{
    ///Clave con la que se envía la canción al navegar.
    public const string SongKey = "Song";

    private readonly INavigationService _navigation;

    private Song? _song;
    private bool _isPlaying;
    private double _progress;
    private string _positionText = "0:00";
    private string _durationText = "0:30";

    public PlayerViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        TogglePlayCommand = new Command(() => IsPlaying = !IsPlaying);
        BackCommand = new Command(async () => await _navigation.GoToAsync(".."));
    }

    public ICommand TogglePlayCommand { get; }
    public ICommand BackCommand { get; }

    // Propiedades de solo lectura que salen de la canción recibida.
    public string Title => _song?.Title ?? string.Empty;
    public string Artist => _song?.Artist ?? string.Empty;
    public string Album => _song?.Album ?? string.Empty;
    public string ArtworkUrl => _song?.ArtworkUrl ?? string.Empty;
    public string PreviewUrl => _song?.PreviewUrl ?? string.Empty;

    public bool IsPlaying
    {
        get => _isPlaying;
        private set
        {
            if (SetProperty(ref _isPlaying, value))
                OnPropertyChanged(nameof(PlayButtonText));   // El texto del botón depende de IsPlaying
        }
    }

    public string PlayButtonText => IsPlaying ? "Pausar" : "Reproducir";

    /// Progreso de 0 a 1, enlazado a la barra.
    public double Progress
    {
        get => _progress;
        private set => SetProperty(ref _progress, value);
    }

    public string PositionText
    {
        get => _positionText;
        private set => SetProperty(ref _positionText, value);
    }

    public string DurationText
    {
        get => _durationText;
        private set => SetProperty(ref _durationText, value);
    }

   
    /// Shell llama a este método al navegar y entrega los parámetros.
    /// Así recibimos la canción elegida sin acoplar las pantallas.
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(SongKey, out object? value) && value is Song song)
        {
            _song = song;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Artist));
            OnPropertyChanged(nameof(Album));
            OnPropertyChanged(nameof(ArtworkUrl));
            OnPropertyChanged(nameof(PreviewUrl));
        }
    }

    public void StartPlayback() => IsPlaying = true;

    public void StopPlayback() => IsPlaying = false;

    ///La vista nos informa de la posición y calculamos progreso y textos.
    public void UpdatePosition(TimeSpan position, TimeSpan duration)
    {
        PositionText = FormatTime(position);
        DurationText = FormatTime(duration);
        Progress = duration.TotalSeconds > 0
            ? Math.Clamp(position.TotalSeconds / duration.TotalSeconds, 0, 1)
            : 0;
    }

    /// Cuando termina el preview volvemos al estado inicial.
    public void OnPlaybackEnded()
    {
        IsPlaying = false;
        Progress = 0;
        PositionText = FormatTime(TimeSpan.Zero);
    }

    // Convierte un TimeSpan en texto "m:ss".
    private static string FormatTime(TimeSpan time) => $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
}