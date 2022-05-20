using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfCalculator.Common.MVVM;

/// <summary>
/// Base view model.
/// </summary>
public abstract class ViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Propery changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// On property changed.
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Set field.
    /// </summary>
    /// <typeparam name="T">Field</typeparam>
    /// <param name="field">Field</param>
    /// <param name="value">Value</param>
    /// <param name="propertyName">PropertyName</param>
    /// <returns>True if property is changed</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (propertyName == null)
        {
            return false;
        }

        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

}
