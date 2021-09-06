using Modellgleis.UndoRedo.Example.Commands.Callbacks;
using System;
using System.Windows.Input;

namespace Modellgleis.UndoRedo.Example.Commands
{
    public interface IRepeatableCommand : ICommand
    {
        event EventHandler<CommandExecutedEventArgs> CommandExecuted;
        event EventHandler<CommandExecutingEventArgs> CommandExecuting;

        bool IsExecutable { get; set; }
    }
}