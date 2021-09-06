using Modellgleis.UndoRedo.Example.Commands;
using System.ComponentModel;

namespace Modellgleis.UndoRedo.Example.Data
{
    public class Person : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ValueCommand<int> _setValueCommand;
        private int _pushUpCount;

        public string Name { get; set; }
        public int PushUpCount 
        { 
            get => _pushUpCount;
            set
            {
                SetValueCommand.SetHandler(SetPushUpCount).Execute(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PushUpCount)));
            }
        }
        public ValueCommand<int> SetValueCommand => _setValueCommand ??= new();
        
        private void SetPushUpCount(int value)
        {             
            if ("Chuck Norris".Equals(Name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                _pushUpCount = int.MaxValue;
            }
            else
            {
                _pushUpCount = value;
            }
        }
    }
}
