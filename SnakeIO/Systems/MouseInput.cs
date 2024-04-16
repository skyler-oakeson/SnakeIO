using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the keyboard input of any
    /// entity with KeyboardControllable component.
    /// </summary>
    public class MouseInput : Shared.Systems.System
    {
        MouseState statePrevious;
        Shared.Controls.ControlManager controlManager;

        public MouseInput(Shared.Controls.ControlManager controlManager)
            : base(
                   typeof(Shared.Components.MouseControllable)
                   )
        {
            this.controlManager = controlManager;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                MouseUpdate(entity, elapsedTime);
            }
        }


        /// <summary>
        /// Goes through all the registered commands and invokes the callbacks if they
        /// are active.
        /// </summary>
        public void MouseUpdate(Shared.Entities.Entity entity, TimeSpan elapsedTime)
        {
            Shared.Components.MouseControllable mCon = entity.GetComponent<Shared.Components.MouseControllable>();
            MouseState state = Mouse.GetState();
            foreach (Shared.Controls.ControlContext control in mCon.controls.Keys)
            {
                mCon.controls[control](entity, elapsedTime, state.X, state.Y);
            }
            // Move the current state to the previous state for the next time around
            statePrevious = state;
        }
    }
}
