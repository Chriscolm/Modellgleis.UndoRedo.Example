using Modellgleis.UndoRedo.Example.Commands.Callbacks;
using System;
using System.Windows.Input;

namespace Modellgleis.UndoRedo.Example.Commands
{
    public class RelayCommand : IRepeatableCommand
    {
        public event EventHandler<CommandExecutingEventArgs> CommandExecuting;
        public event EventHandler<CommandExecutedEventArgs> CommandExecuted;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public bool IsExecutable { get; set; } // hier ohne Bedeutung

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            _canExecute = canExecute;
        }

        public virtual bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            CommandExecuting?.Invoke(this, new CommandExecutingEventArgs());

            _execute?.Invoke(parameter);

            CommandExecuted?.Invoke(this, new CommandExecutedEventArgs());
        }
    }
}
