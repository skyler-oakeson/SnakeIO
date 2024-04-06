using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shared.Controls
{
    public delegate void ControlDelegate(TimeSpan ElapsedTime, float value);
    public delegate void ControlDelegatePosition(TimeSpan ElapsedTime, int x, int y);

    public class ControlManager
    {
        private Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>> controls { get; set; } =
            new Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>>();
        private Dictionary<Keys, ControlDelegate> delegates { get; set; } =
            new Dictionary<Keys, ControlDelegate>();
        private Dictionary<Controls.MouseEvent, ControlDelegatePosition> delegatesPosition { get; set; } =
            new Dictionary<Controls.MouseEvent, ControlDelegatePosition>();
        private DataManager dataManager;
        private KeyboardState statePrevious;
        private MouseState mouseStatePrevious;

        public ControlManager(DataManager dm)
        {
            this.dataManager = dm;
            controls = dm.Load<Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>>>(controls);
        }

        public void RegisterControl<T>(Control con, T d)
        {
            RegisterScene(con.sc);
            // If the control hasn't been loaded register it
            if (!controls[con.sc].ContainsKey(con.cc))
            {
                controls[con.sc].Add(con.cc, con);
            }
            // Loaded control will override the register so it will only be defaulted if it wasn't able to load
            con = controls[con.sc][con.cc];
            if (d is ControlDelegate)
                delegates.Add((Keys)con.key, d as ControlDelegate);
            else if (d is ControlDelegatePosition)
                delegatesPosition.Add((Controls.MouseEvent)con.mouseEvent, d as ControlDelegatePosition);
        }

        private void RegisterScene(Scenes.SceneContext sc)
        {
            // If Scene hasn't been registered or wasn't loaded
            if (!controls.ContainsKey(sc))
            {
                controls.Add(sc, new Dictionary<ControlContext, Control>());
            }
        }

        /// <summary>
        /// Changes the key used for a registered control and changes what key references the delegate associated with it.
        /// <summary>
        public void ChangeKey(Scenes.SceneContext sc, ControlContext cc, Keys key)
        {
            Keys old = (Keys)controls[sc][cc].key;
            controls[sc][cc].key = key;
            ControlDelegate ce = delegates[old];
            delegates.Remove(key);
            delegates.Add(key, ce);
            SaveKeys();
        }

        /// <summary>
        /// Returns a key based on the scene and the control context provided.
        /// <summary>
        public Keys GetKey(Scenes.SceneContext sc, ControlContext cc)
        {
            return (Keys)controls[sc][cc].key;
        }

        /// <summary>
        /// Saves the registered controls to file.
        /// <summary>
        public void SaveKeys()
        {
            dataManager.Save<Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>>>(controls);
        }

        /// <summary>
        /// Goes through all the registered commands and invokes the callbacks if they
        /// are active.
        /// </summary>
        public void Update(TimeSpan elapsedTime, Scenes.SceneContext sc)
        {
            Dictionary<ControlContext, Control> sceneControls = controls[sc];
            KeyboardState state = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            foreach (Control control in sceneControls.Values)
            {
                if (control.mouseEvent != null)
                {
                    if (delegatesPosition.ContainsKey((Controls.MouseEvent)control.mouseEvent))
                    {
                        delegatesPosition[(Controls.MouseEvent)control.mouseEvent](elapsedTime, mouseState.X, mouseState.Y);
                    }
                }
                if (control.key != null)
                {
                    if (delegates.ContainsKey((Keys)control.key) && !control.keyPressOnly && state.IsKeyDown((Keys)control.key))
                    {
                        delegates[(Keys)control.key](elapsedTime, 1.0f);
                    }
                    else if (!control.keyPressOnly && state.IsKeyDown((Keys)control.key))
                    {
                        delegates[(Keys)control.key](elapsedTime, 1.0f);
                    }
                }
            }

            //
            // Move the current state to the previous state for the next time around
            statePrevious = state;
            mouseStatePrevious = mouseState;
        }

        /// <summary>
        /// Checks to see if a key was newly pressed.
        /// </summary>
        private bool KeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && !statePrevious.IsKeyDown(key));
        }
    }

}
