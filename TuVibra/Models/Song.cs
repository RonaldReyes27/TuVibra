using System;
using System.Collections.Generic;
using System.Text;

namespace TuVibra.Models;


/// representa una canción. Solo guarda datos,
/// no tiene lógica ni conoce la interfaz.

public class Song
{
    public string Title { get; init; } = string.Empty;
    public string Artist { get; init; } = string.Empty;
    public string Album { get; init; } = string.Empty;
    public string ArtworkUrl { get; init; } = string.Empty;   // Portada
    public string PreviewUrl { get; init; } = string.Empty;   // Audio de 30 s
}