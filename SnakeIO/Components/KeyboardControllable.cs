namespace Components
{
    public class KeyboardControllable : Component
    {
        public KeyboardControllable(
                Controls.ControlManager cm,
                (Controls.Control, Controls.ControlDelegate)[] actions
                )
        {
            foreach ((Controls.Control con, Controls.ControlDelegate del) in actions)
            {
                cm.RegisterControl(con, del);
            }
        }
    }
}
