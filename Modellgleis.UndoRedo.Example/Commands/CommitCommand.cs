using System;
using System.Windows.Input;

namespace Modellgleis.UndoRedo.Example.Commands
{
    public class CommitCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> _execute;

        public CommitCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is T item)
            {
                Execute(item);
            }
        }

        public virtual void Execute(T parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
