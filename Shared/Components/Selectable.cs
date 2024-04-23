#nullable enable

namespace Shared.Components
{
    public delegate bool SelectableDelegate();

    public class Selectable<T> : Component
    {
        public bool interacted { get; set; }
        public bool prevState { get; set; }
        public bool selected { get; set; }
        public T value { get; set; }
        public SelectableDelegate? selectableDelegate { get; set; }

        
        public Selectable(bool selected, T value, SelectableDelegate? selectableDelegate = null)
        {
            this.selected = selected;
            this.interacted = false;
            this.prevState = false;
            this.value = value;
            this.selectableDelegate = selectableDelegate;
        }

        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
