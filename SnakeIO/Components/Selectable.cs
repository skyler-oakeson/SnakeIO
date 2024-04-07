namespace Components
{
    public delegate Scenes.SceneContext SelectionDelegate();

    public class Selectable : Component
    {
        public bool interacted { get; set; }
        public bool selected { get; set; }
        public SelectionDelegate selectionDelegate { get; set; }
        
        public Selectable(bool selected, SelectionDelegate selectionDelegate)
        {
            this.interacted = false;
            this.selected = selected;
            this.selectionDelegate = selectionDelegate;
        }
    }
}
