using Modellgleis.UndoRedo.Example.Data;
using Modellgleis.UndoRedo.Example.Serialization;
using Modellgleis.UndoRedo.Example.UndoRedoStack;
using Modellgleis.UndoRedo.Example.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Modellgleis.UndoRedo.Example
{
    public class Bootstrapper
    {
        public static MainViewModel CreateMainViewModel()
        {
            int maxStackSize = 5;
            ISerializer<IEnumerable<Person>, string> serializer = new JsonSerializer<IEnumerable<Person>>();
            IUndoRedoStack<IEnumerable<Person>> undoRedoStack = new UndoRedoStack<IEnumerable<Person>, string>(serializer, maxStackSize);
            var vm = new MainViewModel(undoRedoStack);
            return vm;
        }
    }
}
