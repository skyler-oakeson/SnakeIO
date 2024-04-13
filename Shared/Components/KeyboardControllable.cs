using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping keyboard control data of
    /// entites that are controllable using a keyboard.
    /// </summary>

    public class KeyboardControllable : Shared.Components.Component
    {
        public Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate> controls = new Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate>();
        public Shared.Controls.ControlableEntity entityType;
        public bool enable;

        public KeyboardControllable(
                bool enable,
                Shared.Controls.ControlableEntity entityType,
                Shared.Controls.ControlManager cm,
                (Shared.Controls.ControlContext, Shared.Controls.ControlDelegate)[] actions
                )
        {
            this.enable = enable;
            this.entityType = entityType;
            foreach ((Shared.Controls.ControlContext con, Shared.Controls.ControlDelegate del) in actions)
            {
                controls.Add(con, del);
            }
        }

        // Input will be changing, do this with changed input
        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(enable));
            data.AddRange(BitConverter.GetBytes((UInt16)entityType));
        }
    }
}
