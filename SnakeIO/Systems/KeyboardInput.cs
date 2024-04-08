using System;
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
    public class KeyboardInput : Shared.Systems.System
    {
        KeyboardState statePrevious;
        Shared.Controls.ControlManager controlManager;

        public KeyboardInput(Shared.Controls.ControlManager controlManager)
            : base(
                   typeof(Shared.Components.KeyboardControllable)
                   )
        {
            this.controlManager = controlManager;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                if (entity.GetComponent<Shared.Components.KeyboardControllable>().enable)
                {
                    KeyboardUpdate(entity, elapsedTime);
                }
            }
        }


        /// <summary>
        /// Goes through all the registered commands and invokes the callbacks if they
        /// are active.
        /// </summary>
        public void KeyboardUpdate(Shared.Entities.Entity entity, TimeSpan elapsedTime)
        {
            Shared.Components.KeyboardControllable kCon = entity.GetComponent<Shared.Components.KeyboardControllable>();
            KeyboardState state = Keyboard.GetState();
            foreach (Shared.Controls.ControlContext control in kCon.controls.Keys)
            {
                Shared.Controls.Control controlSettings = controlManager.GetControl(control);
                if (!(bool)controlSettings.keyPressOnly && state.IsKeyDown((Keys)controlSettings.key))
                {
                    kCon.controls[control](elapsedTime, 1.0f);
                }
                else if ((bool)controlSettings.keyPressOnly && KeyPressed((Keys)controlSettings.key))
                {
                    kCon.controls[control](elapsedTime, 1.0f);
                }
            }

            // Move the current state to the previous state for the next time around
            statePrevious = state;
        }

        // <summary>
        // Checks to see if a key was newly pressed.
        // </summary>
        //
        private bool KeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !statePrevious.IsKeyDown(key));
        }
    }
}
