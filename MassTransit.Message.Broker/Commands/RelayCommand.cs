using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MassTransit.Message.Broker.Commands;

internal class RelayCommand : ICommand
{

    readonly Action? _executeNoParam;
    readonly Action<object?>? _executeWithParam;
    readonly Predicate<object?>? _canExecute;

    public RelayCommand(Action execute) : this(execute, null) { }
    public RelayCommand(Action execute, Predicate<object?>? canExecute)
    {
        _executeNoParam = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Action<object?> execute) : this(execute, null) { }
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
    {
        _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    [DebuggerStepThrough]
    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public void Execute() => _executeNoParam?.Invoke();
    public void Execute(object? parameter)
    {
        if (_executeNoParam != null)
        {
            Execute();
            return;
        }

        _executeWithParam?.Invoke(parameter);
    }

}
