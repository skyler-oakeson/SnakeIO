namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping mouse control data of
    /// entites that are controllable using a keyboard.
    /// </summary>
    public class MouseControllable : Component
    {
        public (Shared.Controls.Control, Shared.Controls.ControlDelegatePosition)[] actions;
        public MouseControllable(
                Shared.Controls.ControlManager cm,
                (Shared.Controls.Control, Shared.Controls.ControlDelegatePosition)[] actions
                )
        {
            this.actions = actions;
            foreach ((Shared.Controls.Control con, Shared.Controls.ControlDelegatePosition del) in actions)
            {
                cm.RegisterControl(con, del);
            }
        }
    }
}
