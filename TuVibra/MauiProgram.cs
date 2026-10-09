using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;
using TuVibra.Services;
using TuVibra.ViewModels;

namespace TuVibra;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkitMediaElement(false) // Sin parámetro booleano
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servicios
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
        builder.Services.AddSingleton<IMusicService, ITunesMusicService>();

        // ViewModels y páginas
        builder.Services.AddTransient<SplashViewModel>();
        builder.Services.AddTransient<SplashPage>();
        builder.Services.AddTransient<SearchViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<PlayerViewModel>();
        builder.Services.AddTransient<PlayerPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}