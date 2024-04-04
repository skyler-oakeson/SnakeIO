using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shared.Controls
{
    public delegate void ControlDelegate(GameTime gameTime, float value);
    public delegate void ControlDelegatePosition(GameTime GameTime, int x, int y);

    public class ControlManager
    {
        private Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>> controls { get; set; } =
            new Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>>();
        private Dictionary<Keys, ControlDelegate> delegates { get; set; } =
            new Dictionary<Keys, ControlDelegate>();
        private DataManager dataManager;
        private KeyboardState statePrevious;

        public ControlManager(DataManager dm)
        {
            this.dataManager = dm;
            controls = dm.Load<Dictionary<Scenes.SceneContext, Dictionary<ControlContext, Control>>>(controls);
        }

        public void RegisterControl(Control con, ControlDelegate d)
        {
            RegisterScene(con.sc);
            // If the control hasn't been loaded register it
            if (!controls[con.sc].ContainsKey(con.cc))
            {
                controls[con.sc].Add(con.cc, con);
            }
            // Loaded control will override the register so it will only be defaulted if it wasn't able to load
            con = controls[con.sc][con.cc];
            delegates.Add(con.key, d);
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
            Keys old = controls[sc][cc].key;
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
            return controls[sc][cc].key;
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
        public void Update(GameTime gameTime, Scenes.SceneContext sc)
        {
            Dictionary<ControlContext, Control> sceneControls = controls[sc];
            KeyboardState state = Keyboard.GetState();
            foreach (Control control in sceneControls.Values)
            {
                if (control.keyPressOnly && KeyPressed(control.key))
                {
                    delegates[control.key](gameTime, 1.0f);
                }
                else if (!control.keyPressOnly && state.IsKeyDown(control.key))
                {
                    delegates[control.key](gameTime, 1.0f);
                }
            }

            //
            // Move the current state to the previous state for the next time around
            statePrevious = state;
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
