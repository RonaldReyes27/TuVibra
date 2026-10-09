namespace TuVibra;


/// Clase principal de la aplicación. Su única tarea es crear la ventana
/// y arrancar con el Shell, que a su vez muestra primero el splash.

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}