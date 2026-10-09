namespace TuVibra;


/// Shell de la aplicación. Además de inicializar el XAML, registra las rutas
/// de las pantallas a las que se llega "por encima" (no están en el XAML).

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Ruta usada en SearchViewModel para abrir el reproductor.
        Routing.RegisterRoute("PlayerPage", typeof(PlayerPage));
    }
}