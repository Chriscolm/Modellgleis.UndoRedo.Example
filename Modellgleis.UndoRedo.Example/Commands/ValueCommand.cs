using Modellgleis.UndoRedo.Example.Commands.Callbacks;
using System;

namespace Modellgleis.UndoRedo.Example.Commands
{
    public class ValueCommand<T> : CommitCommand<T>, IRepeatableCommand
    {
        public event EventHandler<CommandExecutingEventArgs> CommandExecuting;
        public event EventHandler<CommandExecutedEventArgs> CommandExecuted;

        private Action<T> _execute;

        public bool IsExecutable { get; set; } // hier ohne Bedeutung

        public ValueCommand() : base(null)
        {

        }

        public ValueCommand<T> SetHandler(Action<T> execute)
        {
            _execute = execute;
            return this;
        }

        public override bool CanExecute(object parameter)
        {
            return _execute != null;
        }

        public override void Execute(T parameter)
        {
            CommandExecuting?.Invoke(this, new());

            _execute.Invoke(parameter);

            CommandExecuted?.Invoke(this, new());
        }
    }
}
