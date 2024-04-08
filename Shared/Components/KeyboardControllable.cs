namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping keyboard control data of
    /// entites that are controllable using a keyboard.
    /// </summary>
    public class KeyboardControllable : Component
    {
        public (Controls.Control, Controls.ControlDelegate)[] actions;
        public KeyboardControllable(
                Controls.ControlManager cm,
                (Controls.Control, Controls.ControlDelegate)[] actions
                )
        {
            foreach ((Controls.Control con, Controls.ControlDelegate del) in actions)
            {
                cm.RegisterControl(con, del);
            }
            this.actions = actions;
        }

        // Input will be changing, do this with changed input
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
