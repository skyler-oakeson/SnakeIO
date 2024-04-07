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
    public class KeyboardInput : System
    {
        KeyboardState statePrevious;
        ControlManager controlManager;

        public KeyboardInput(ControlManager controlManager)
            : base(
                   typeof(Components.KeyboardControllable)
                   )
        {
            this.controlManager = controlManager;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                if (entity.GetComponent<KeyboardControllable>().enable)
                {
                    KeyboardUpdate(entity, gameTime);
                }
            }
        }


        /// <summary>
        /// Goes through all the registered commands and invokes the callbacks if they
        /// are active.
        /// </summary>
        public void KeyboardUpdate(Entities.Entity entity, GameTime gameTime)
        {
            KeyboardControllable kCon = entity.GetComponent<KeyboardControllable>();
            KeyboardState state = Keyboard.GetState();
            foreach (ControlContext control in kCon.controls.Keys)
            {
                Control controlSettings = controlManager.GetControl(control);
                if (!(bool)controlSettings.keyPressOnly && state.IsKeyDown((Keys)controlSettings.key))
                {
                    kCon.controls[control](gameTime, 1.0f);
                }
                else if ((bool)controlSettings.keyPressOnly && KeyPressed((Keys)controlSettings.key))
                {
                    kCon.controls[control](gameTime, 1.0f);
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
