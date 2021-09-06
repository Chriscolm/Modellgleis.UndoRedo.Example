using System.Collections.Generic;
using System.Linq;

namespace Modellgleis.UndoRedo.Example.UndoRedoStack
{
    public class MaxSizedStack<T>
    {
        private readonly int _maxItems;
        private readonly List<T> _collection;

        public MaxSizedStack(int maxItems)
        {
            _maxItems = maxItems;
            _collection = new(); // seit C#-9 geht so etwas - der Compiler erkennt, dass new List<T>() gemeint ist
        }

        public void Push(T item)
        {
            if (_collection.Count > _maxItems - 1)
            {
                _collection.RemoveAt(0);
            }
            _collection.Add(item);
        }

        public bool TryPeek(out T item)
        {
            if (_collection.Any())
            {
                item = _collection[^1]; // neu in C#-8: Ende-Operator ^, bezeichnet den Index relativ vom Ende der Sequenz, also gleichbedeutend mit collection.Count - 1
                return true;
            }
            item = default;
            return false;
        }

        public T Pop()
        {
            var item = _collection[^1];
            _collection.Remove(item);
            return item;
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}
