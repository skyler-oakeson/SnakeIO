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
            Shared.Components.KeyboardControllable kCon = entity.GetComponent<Shared.Components.KeyboardControllable>();
            KeyboardState state = Keyboard.GetState();
            foreach (Shared.Controls.ControlContext control in kCon.controls.Keys)
            {
                Shared.Controls.Control controlSettings = controlManager.GetControl(control);
                if (!(bool)controlSettings.keyPressOnly && state.IsKeyDown((Keys)controlSettings.key))
                {
                    kCon.controls[control](entity, elapsedTime);
                    List<Shared.Controls.ControlContext> inputs = new List<Shared.Controls.ControlContext>();
                    inputs.Add(control);
                    SnakeIO.MessageQueueClient.instance.sendMessageWithId(new Shared.Messages.Input(entity.id, inputs, elapsedTime));
                }
                else if ((bool)controlSettings.keyPressOnly && KeyPressed((Keys)controlSettings.key))
                {
                    kCon.controls[control](entity, elapsedTime);
                    List<Shared.Controls.ControlContext> inputs = new List<Shared.Controls.ControlContext>();
                    inputs.Add(control);
                    SnakeIO.MessageQueueClient.instance.sendMessageWithId(new Shared.Messages.Input(entity.id, inputs, elapsedTime));
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
