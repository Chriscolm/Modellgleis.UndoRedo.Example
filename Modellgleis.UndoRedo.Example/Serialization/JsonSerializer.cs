using System.Text.Json;

namespace Modellgleis.UndoRedo.Example.Serialization
{
    public class JsonSerializer<TItem> : ISerializer<TItem, string>
    {
        public TItem Deserialize(string serializedValue)
        {
            var item = JsonSerializer.Deserialize<TItem>(serializedValue);
            return item;
        }

        public string Serialize(TItem item)
        {
            var s = JsonSerializer.Serialize(item);
            return s;
        }
    }
}
