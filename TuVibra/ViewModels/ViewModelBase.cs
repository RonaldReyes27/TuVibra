using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TuVibra.ViewModels;


/// Clase base de todos los ViewModels. Implementa INotifyPropertyChanged,
/// que es lo que permite que la vista se actualice sola cuando cambia una propiedad.
/// Está aquí una sola vez para no repetir este código en cada ViewModel (DRY).

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

  
    /// Asigna el valor solo si cambió y avisa a la vista.
    /// [CallerMemberName] rellena automáticamente el nombre de la propiedad.
  
    protected bool SetProperty<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}