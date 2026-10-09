using System.Text.Json;
using System.Text.Json.Serialization;
using TuVibra.Models;

namespace TuVibra.Services;


/// Implementación de IMusicService usando la iTunes Search API.
/// Única responsabilidad: hablar con la API y convertir su JSON en objetos Song.

public class ITunesMusicService : IMusicService
{
    private const string BaseUrl = "https://itunes.apple.com/search";
    private const int MaxResults = 20;

    private readonly HttpClient _http;

    // HttpClient llega por inyección de dependencias.
    public ITunesMusicService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<Song>> SearchAsync(string term, CancellationToken cancellationToken = default)
    {
        // Uri.EscapeDataString convierte espacios y símbolos para que la URL sea válida.
        string url = $"{BaseUrl}?term={Uri.EscapeDataString(term)}&media=music&entity=song&limit={MaxResults}";

        using HttpResponseMessage response = await _http.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();   // Lanza excepción si la API responde con error

        string json = await response.Content.ReadAsStringAsync(cancellationToken);
        ITunesResponse? data = JsonSerializer.Deserialize<ITunesResponse>(json);

        if (data is null)
            return Array.Empty<Song>();

        // Solo dejamos canciones que tengan preview, porque la app trata de eso.
        return data.Results
            .Where(t => !string.IsNullOrEmpty(t.PreviewUrl))
            .Select(t => new Song
            {
                Title = t.TrackName ?? string.Empty,
                Artist = t.ArtistName ?? string.Empty,
                Album = t.CollectionName ?? string.Empty,
                // La API da portada de 100x100; pedimos una de 300x300 para que se vea nítida.
                ArtworkUrl = (t.ArtworkUrl100 ?? string.Empty).Replace("100x100", "300x300"),
                PreviewUrl = t.PreviewUrl ?? string.Empty
            })
            .ToList();
    }

    // Clases privadas que reflejan el JSON de iTunes (DTOs). Quedan ocultas
    // dentro del servicio: el resto de la app solo conoce Song.
    private sealed class ITunesResponse
    {
        [JsonPropertyName("results")]
        public List<ITunesTrack> Results { get; set; } = new();
    }

    private sealed class ITunesTrack
    {
        [JsonPropertyName("trackName")] public string? TrackName { get; set; }
        [JsonPropertyName("artistName")] public string? ArtistName { get; set; }
        [JsonPropertyName("collectionName")] public string? CollectionName { get; set; }
        [JsonPropertyName("artworkUrl100")] public string? ArtworkUrl100 { get; set; }
        [JsonPropertyName("previewUrl")] public string? PreviewUrl { get; set; }
    }
}