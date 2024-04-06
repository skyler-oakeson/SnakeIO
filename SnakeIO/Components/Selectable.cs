namespace Components
{
    public delegate Scenes.SceneContext SelectionDelegate();

    public class Selectable : Component
    {
        public bool selected;
        public bool highlighted;
        public SelectionDelegate selectionDelegate;
        
        public Selectable(bool highlighted, SelectionDelegate selectionDelegate)
        {
            this.selected = false;
            this.highlighted = highlighted;
            this.selectionDelegate = selectionDelegate;
        }
    }
}
