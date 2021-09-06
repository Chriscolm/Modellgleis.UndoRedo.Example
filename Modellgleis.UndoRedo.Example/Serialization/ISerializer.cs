namespace Modellgleis.UndoRedo.Example.Serialization
{
    public interface ISerializer<TItem, TSerialized>
    {
        TSerialized Serialize(TItem item);
        TItem Deserialize(TSerialized serializedValue);
    }
}
