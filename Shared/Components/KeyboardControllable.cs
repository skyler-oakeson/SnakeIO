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
        public bool enable;

        public KeyboardControllable(
                bool enable,
                Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate> controls 
                )
        {
            this.enable = enable;
            this.controls = controls;
        }

        // Input will be changing, do this with changed input
        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(enable));
        }
    }
}
