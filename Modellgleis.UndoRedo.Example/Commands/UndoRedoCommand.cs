using System;

namespace Modellgleis.UndoRedo.Example.Commands
{
    public class UndoRedoCommand : RelayCommand
    {
        public UndoRedoCommand(Action<object> execute) : base(execute)
        {            
        }

        public UndoRedoCommand(Action<object> execute, Func<object, bool> canExecute): base(execute, canExecute)
        {
            
        }

        public override bool CanExecute(object parameter)
        {
            return IsExecutable;
        }
    }
}
