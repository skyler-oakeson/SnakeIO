using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Controls;
using Components;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the keyboard input of any
    /// entity with KeyboardControllable component.
    /// </summary>
    public class MouseInput : System
    {
        MouseState statePrevious;
        ControlManager controlManager;

        public MouseInput(ControlManager controlManager)
            : base(
                   typeof(Components.MouseControllable)
                   )
        {
            this.controlManager = controlManager;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                MouseUpdate(entity, gameTime);
            }
        }


        /// <summary>
        /// Goes through all the registered commands and invokes the callbacks if they
        /// are active.
        /// </summary>
        public void MouseUpdate(Entities.Entity entity, GameTime gameTime)
        {
            MouseControllable mCon = entity.GetComponent<MouseControllable>();
            MouseState state = Mouse.GetState();
            foreach (ControlContext control in mCon.controls.Keys)
            {
                mCon.controls[control](gameTime, state.X, state.Y);
            }
            // Move the current state to the previous state for the next time around
            statePrevious = state;
        }
    }
}
