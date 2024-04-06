namespace Components
{
    /// <summary>
    /// This component is responsible for keeping mouse control data of
    /// entites that are controllable using a keyboard.
    /// </summary>
    public class MouseControllable : Component
    {
        public MouseControllable(
                Controls.ControlManager cm,
                (Controls.Control, Controls.ControlDelegatePosition)[] actions
                )
        {
            foreach ((Controls.Control con, Controls.ControlDelegatePosition del) in actions)
            {
                cm.RegisterControl(con, del);
            }
        }
    }
}
