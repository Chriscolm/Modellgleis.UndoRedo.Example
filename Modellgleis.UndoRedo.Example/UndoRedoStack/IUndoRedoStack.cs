using Modellgleis.UndoRedo.Example.Commands;
using System;

namespace Modellgleis.UndoRedo.Example.UndoRedoStack
{
    public interface IUndoRedoStack<T>
    {
        void RegisterCommand(IRepeatableCommand command);
        void RegisterUndoCommand(IRepeatableCommand command);
        void RegisterRedoCommand(IRepeatableCommand command);
        void RegisterStateGetter(Func<T> stateGetter);
        void RegisterCommitCommand(CommitCommand<T> command);
        void UnregisterCommand(IRepeatableCommand command);
        void Clear();
    }
}
