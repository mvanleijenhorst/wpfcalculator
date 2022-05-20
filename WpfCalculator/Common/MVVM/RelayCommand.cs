using System;
using System.Windows.Input;

namespace WpfCalculator.Common.MVVM;

/// <summary>
/// Relay command.
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    /// Action.
    /// </summary>
    readonly Action<object?> _execute;
    readonly Predicate<object?>? _canExecute;

    /// <summary>
    /// Can execute.
    /// </summary>
    /// <param name="execute"></param>
    public RelayCommand(Action<object?> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="execute"><see cref="Action{T}/> of <see cref="object"/></param>
    /// <param name="canExecute"><see cref="Predicate{T}"/> of <see cref="object"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Can execute.
    /// </summary>
    /// <param name="parameter"><see cref="object"/> parameter</param>
    /// <returns>Return true if it can execute</returns>
    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

    /// <summary>
    /// Can excute changed event handler.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Excute command.
    /// </summary>
    /// <param name="parameter"><see cref="object"/> parameter</param>
    public void Execute(object? parameter) => _execute(parameter);
}
