using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Controls;

namespace Components
{
    /// <summary>
    /// This component is responsible for keeping keyboard control data of
    /// entites that are controllable using a keyboard.
    /// </summary>

    public class KeyboardControllable : Component
    {
        public Dictionary<ControlContext, ControlDelegate> controls = new Dictionary<ControlContext, ControlDelegate>();

        public KeyboardControllable(
                ControlManager cm,
                (ControlContext, ControlDelegate)[] actions
                )
        {
            foreach ((ControlContext con, ControlDelegate del) in actions)
            {
                controls.Add(con, del);
            }
        }
    }
}
