using TuVibra.Models;
using TuVibra.ViewModels;

namespace TuVibra;


/// Vista de la búsqueda. Conecta el ViewModel y traduce el toque sobre una
/// canción en una orden para el ViewModel.

public partial class MainPage : ContentPage
{
    private readonly SearchViewModel _viewModel;

    public MainPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Se ejecuta al tocar una canción de la lista.
    private async void OnSongSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Song song)
            return;   // También se dispara al limpiar la selección: lo ignoramos

        // Limpiamos la selección para poder volver a tocar la misma canción después.
        ((CollectionView)sender!).SelectedItem = null;

        await _viewModel.OpenPlayerAsync(song);
    }
}