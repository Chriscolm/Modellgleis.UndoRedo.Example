using Modellgleis.UndoRedo.Example.Commands;
using Modellgleis.UndoRedo.Example.Serialization;
using System;

namespace Modellgleis.UndoRedo.Example.UndoRedoStack
{
    public class UndoRedoStack<T, TState> : IUndoRedoStack<T>
    {
        private readonly MaxSizedStack<TState> _undoStack;
        private readonly MaxSizedStack<TState> _redoStack;
        private readonly ISerializer<T, TState> _serializer;
        private IRepeatableCommand _redoCommand;
        private IRepeatableCommand _undoCommand;
        private Func<T> _stateGetter;
        private CommitCommand<T> _commitCommand;

        public UndoRedoStack(ISerializer<T, TState> serializer, int maxItems)
        {
            _undoStack = new(maxItems);
            _redoStack = new(maxItems);
            _serializer = serializer;
        }

        /// <summary>
        /// Registrieren des Wiederholen-Befehls
        /// </summary>
        /// <param name="command"></param>
        public void RegisterRedoCommand(IRepeatableCommand command)
        {
            _redoCommand = command;
            command.CommandExecuting += OnRedoCommandExecuting;
            command.CommandExecuted += OnUndoOrRedoCommandExecuted;
        }

        /// <summary>
        /// Registrieren des Rückgängig-Befehls
        /// </summary>
        /// <param name="command"></param>
        public void RegisterUndoCommand(IRepeatableCommand command)
        {
            _undoCommand = command;
            command.CommandExecuting += OnUndoCommandExecuting;
            command.CommandExecuted += OnUndoOrRedoCommandExecuted;
        }

        public void RegisterStateGetter(Func<T> stateGetter)
        {
            _stateGetter = stateGetter;
        }

        /// <summary>
        /// Registrieren eines Befehls, der rückgängig gemacht oder wiederholt werden kann
        /// </summary>
        /// <param name="name"></param>
        /// <param name="command"></param>
        public void RegisterCommand(IRepeatableCommand command)
        {
            command.CommandExecuting += OnCommandExecuting;
            command.CommandExecuted += OnCommandExecuted;
        }

        public void RegisterCommitCommand(CommitCommand<T> command)
        {
            _commitCommand = command;
        }

        private void OnCommandExecuting(object sender, Commands.Callbacks.CommandExecutingEventArgs e)
        {
            // Daten bevor Befehl ausgeführt wird in _undoStack schreiben
            var current = _stateGetter.Invoke();
            var state = _serializer.Serialize(current);
            _undoStack.Push(state);
        }

        private void OnCommandExecuted(object sender, Commands.Callbacks.CommandExecutedEventArgs e)
        {
            _undoCommand.IsExecutable = _undoStack.TryPeek(out var _);
        }

        private void OnUndoCommandExecuting(object sender, Commands.Callbacks.CommandExecutingEventArgs e)
        {
            // Rückgängig: Daten vor Ausführen des Befehls in _redoStack schreiben
            var current = _stateGetter.Invoke();
            var state = _serializer.Serialize(current);
            _redoStack.Push(state);
            // letzten Zustand vom _undoStack holen
            var s = _undoStack.Pop();
            var item = _serializer.Deserialize(s);
            // Commit ausführen
            Act(item);
        }

        private void OnRedoCommandExecuting(object sender, Commands.Callbacks.CommandExecutingEventArgs e)
        {
            // Wiederholen: Daten vor Ausführen des Befehls in _undoStack schreiben
            var current = _stateGetter.Invoke();
            var state = _serializer.Serialize(current);
            _undoStack.Push(state);
            // letzten Zustand vom Redo-Stack holen
            var s = _redoStack.Pop();
            var item = _serializer.Deserialize(s);
            // Commit ausführen
            Act(item);
        }

        private void Act(T item)
        {
            _commitCommand.Execute(item);
        }

        private void OnUndoOrRedoCommandExecuted(object sender, Commands.Callbacks.CommandExecutedEventArgs e)
        {
            // Nach Ausführen eines Rückgängig- oder Wiederholen-Befehls prüfen, ob noch weitere Rückgängig- oder Wiederholen-Schritte verfügbar sind
            _undoCommand.IsExecutable = _undoStack.TryPeek(out var _);
            _redoCommand.IsExecutable = _redoStack.TryPeek(out var _);
        }

        public void UnregisterCommand(IRepeatableCommand command)
        {
            command.CommandExecuting -= OnCommandExecuting;
            command.CommandExecuted -= OnCommandExecuted;
        }

        public void Clear()
        {
            _undoStack.Clear();
            _undoCommand.IsExecutable = false;
            _redoStack.Clear();
            _redoCommand.IsExecutable = false;
        }
    }
}
