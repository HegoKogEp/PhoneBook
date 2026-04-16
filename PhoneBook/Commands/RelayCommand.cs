using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PhoneBook.Commands
{
    // no parameter
    /// <summary>
    /// Класс RelayCommand, реализующий интерфейс <see cref="ICommand"/>, который позволяет создавать команды для привязки к элементам управления в WPF,
    /// не требующие параметров.
    /// </summary>
    /// <param name="execute">Делегат, представляющий метод, который будет выполнен при вызове команды.</param>
    /// <param name="canExecute">Делегат, представляющий метод, который определяет, может ли команда выполняться.</param>
    public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
    {
        private readonly Action _execute = execute;
        private readonly Func<bool>? _canExecute = canExecute;
        public bool CanExecute(object? parameter = null)
            => _canExecute?.Invoke() ?? true;
        public void Execute(object? parameter)
        {
            if (CanExecute(parameter)) _execute.Invoke();
        }

        public event EventHandler? CanExecuteChanged //Автообновление CanExecute
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

    }
}

// with parameter
/// <summary>
/// Класс RelayCommand<T>, реализующий интерфейс <see cref="ICommand"/>, который позволяет создавать команды для привязки к элементам управления в WPF,
/// требующие параметры.
/// </summary>
/// <typeparam name="T">Тип параметра команды.</typeparam>
/// <param name="execute">Делегат, представляющий метод, который будет выполнен при вызове команды.</param>
/// <param name="canExecute">Делегат, представляющий метод, который определяет, может ли команда выполняться.</param>
public class RelayCommand<T>(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand
{
    private readonly Action<object?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Predicate<object?>? _canExecute = canExecute;
    public bool CanExecute(object? parameter)
        => _canExecute?.Invoke(parameter) ?? true;
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter)) _execute.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}