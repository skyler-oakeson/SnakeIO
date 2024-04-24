#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
            KeyboardState state = Keyboard.GetState();

            UpdateControllableEntities(entity, state, elapsedTime);
            UpdateLastKeyPress(entity, state);

            // Move the current state to the previous state for the next time around
            statePrevious = state;
        }

        private void UpdateControllableEntities(Shared.Entities.Entity entity, KeyboardState state, TimeSpan elapsedTime)
        {
            Shared.Components.KeyboardControllable kCon = entity.GetComponent<Shared.Components.KeyboardControllable>();
            foreach (Shared.Controls.ControlContext control in kCon.controls.Keys)
            {
                List<Shared.Controls.ControlContext> inputs = new List<Shared.Controls.ControlContext>();
                Shared.Controls.Control controlSettings = controlManager.GetControl(control);
                if (!(bool)controlSettings.keyPressOnly && state.IsKeyDown((Keys)controlSettings.key))
                {
                    inputs.Add(control);
                    kCon.controls[control](entity, elapsedTime);
                }
                else if ((bool)controlSettings.keyPressOnly && KeyPressed((Keys)controlSettings.key))
                {
                    inputs.Add(control);
                    kCon.controls[control](entity, elapsedTime);
                }

                // Send input to server if it is a player
                if (kCon.type == typeof(Shared.Entities.Player))
                {
                    SnakeIO.MessageQueueClient.instance.sendMessageWithId(new Shared.Messages.Input(entity.id, inputs, elapsedTime));
                }
            }
        }

        private void UpdateLastKeyPress(Shared.Entities.Entity entity, KeyboardState state)
        {
            // Saves only the keyboard presses to the KeyboardControllable allowing for text input
            Shared.Components.KeyboardControllable kCon = entity.GetComponent<Shared.Components.KeyboardControllable>();
            kCon.keyPress = null;
            if (state.GetPressedKeys().Length > 0 && KeyPressed(state.GetPressedKeys()[0]))
            {
                Keys newKey = state.GetPressedKeys()[0];
                if (KeyPressed(newKey))
                {
                    kCon.keyPress = newKey;
                }
            }
        }

        // <summary>
        // Checks to see if a key was newly pressed.
        // </summary>
        //
        private bool KeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !statePrevious.IsKeyDown(key));
        }

        public void PrevState(KeyboardState state)
        {
            this.statePrevious = state;
        }
    }
}
