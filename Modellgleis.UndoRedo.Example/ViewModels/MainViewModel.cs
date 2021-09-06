using Modellgleis.UndoRedo.Example.Commands;
using Modellgleis.UndoRedo.Example.Data;
using Modellgleis.UndoRedo.Example.UndoRedoStack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Modellgleis.UndoRedo.Example.ViewModels
{
    public class MainViewModel
    {
        private IRepeatableCommand _removeCommand;
        private IRepeatableCommand _undoCommand;
        private IRepeatableCommand _redoCommand;        
        private CommitCommand<IEnumerable<Person>> _commitCommand;
        private ICommand _reloadCommand;
        private readonly IUndoRedoStack<IEnumerable<Person>> _undoRedoStack;

        public ObservableCollection<Person> Persons { get; private set; }
        public IRepeatableCommand RemoveCommand => _removeCommand ??= new RelayCommand(p => Remove(p), q => CanRemove(q));
        public IRepeatableCommand UndoCommand => _undoCommand ??= new UndoRedoCommand(p => Undo());
        public IRepeatableCommand RedoCommand => _redoCommand ??= new UndoRedoCommand(p => Redo());        
        public CommitCommand<IEnumerable<Person>> CommitCommand => _commitCommand ??= new(p => Commit(p));
        public ICommand ReloadCommand => _reloadCommand ??= new RelayCommand(p => Reload());
        
        public MainViewModel(IUndoRedoStack<IEnumerable<Person>> undoRedoStack)
        {
            _undoRedoStack = undoRedoStack;
            Persons = new ObservableCollection<Person>();
            Load();
            InitUndoRedoStack();
        }

        private void Load()
        {
            var names = new[] { "Axel Schweiß", "Rita Lihn", "Rainer Unsinn", "Christian Koller", "Joe Doe", "Marina D." };
            var r = new Random();
            foreach(var n in names)
            {
                var p = new Person()
                {
                    Name = n,
                    PushUpCount = r.Next(7, 42)
                };
                Persons.Add(p);
            }
            Persons.Add(new Person() { Name = "Chuck Norris", PushUpCount = int.MaxValue });
        }

        private void InitUndoRedoStack()
        {
            _undoRedoStack.RegisterUndoCommand(UndoCommand);
            _undoRedoStack.RegisterRedoCommand(RedoCommand);
            _undoRedoStack.RegisterCommand(RemoveCommand);
            _undoRedoStack.RegisterStateGetter(() =>
            {
                return Persons;
            });
            _undoRedoStack.RegisterCommitCommand(CommitCommand);
            foreach(var p in Persons)
            {
                _undoRedoStack.RegisterCommand(p.SetValueCommand);
            }
        }

        private static bool CanRemove(object parameter)
        {
            if(parameter is Person p)
            {
                return p.Name != "Chuck Norris";
            }
            return true;
        }

        private void Remove(object parameter)
        {
            if(parameter is Person p)
            {
                _undoRedoStack.UnregisterCommand(p.SetValueCommand);
                Persons.Remove(p);
            }
        }

        private static void Undo()
        {
            // das wird hier nicht benötigt, als Konstruktorparameter für UndoCommand könnte auch null verwendet werden
        }
        
        private static void Redo()
        {
            // s. o.
        }

        private void Commit(IEnumerable<Person> parameter)
        {
            var persons = (from q in Persons select q).ToArray();
            foreach (var p in persons)
            {
                Remove(p);
            }            
            foreach(var p in parameter)
            {
                _undoRedoStack.RegisterCommand(p.SetValueCommand);
                Persons.Add(p);
            }
        }

        private void Reload()
        {
            var persons = (from q in Persons select q).ToArray();
            foreach(var p in persons)
            {
                Remove(p);
            }
            _undoRedoStack.Clear();
            Load();
            foreach (var p in Persons)
            {
                _undoRedoStack.RegisterCommand(p.SetValueCommand);
            }
        }
    }
}
