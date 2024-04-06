namespace Components
{
    public class Selectable : Component
    {
        public bool selected;
        
        public Selectable(bool selected)
        {
            this.selected = selected;
        }
    }
}
