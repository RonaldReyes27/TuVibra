using System.Collections.ObjectModel;
using System.Windows.Input;
using TuVibra.Models;
using TuVibra.Services;

namespace TuVibra.ViewModels;


/// Lógica de la pantalla de búsqueda: texto, estado de carga, resultados,
/// mensajes y apertura del reproductor.

public class SearchViewModel : ViewModelBase
{
    // Debe coincidir con la ruta registrada en AppShell.xaml.cs.
    private const string PlayerRoute = "PlayerPage";

    private readonly IMusicService _musicService;
    private readonly INavigationService _navigation;

    private string _searchText = string.Empty;
    private bool _isLoading;
    private string _message = "Busca una canción para empezar";

    public SearchViewModel(IMusicService musicService, INavigationService navigation)
    {
        _musicService = musicService;
        _navigation = navigation;
        SearchCommand = new Command(async () => await SearchAsync());
    }

    public ObservableCollection<Song> Songs { get; } = new();

    public ICommand SearchCommand { get; }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public string Message
    {
        get => _message;
        private set
        {
            if (SetProperty(ref _message, value))
                OnPropertyChanged(nameof(HasMessage));
        }
    }

    public bool HasMessage => !string.IsNullOrEmpty(Message);

    ///Abre el reproductor enviándole la canción elegida.
    public Task OpenPlayerAsync(Song song)
        => _navigation.GoToAsync(PlayerRoute, new Dictionary<string, object>
        {
            [PlayerViewModel.SongKey] = song
        });

    private async Task SearchAsync()
    {
        if (IsLoading)
            return;

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Message = "Escribe el nombre de una canción o artista.";
            return;
        }

        IsLoading = true;
        Message = string.Empty;
        Songs.Clear();

        try
        {
            IReadOnlyList<Song> results = await _musicService.SearchAsync(SearchText.Trim());

            foreach (Song song in results)
                Songs.Add(song);

            if (Songs.Count == 0)
                Message = "No encontramos resultados. Prueba con otra búsqueda.";
        }
        catch (Exception)
        {
            Message = "No se pudo buscar. Revisa tu conexión e intenta de nuevo.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}