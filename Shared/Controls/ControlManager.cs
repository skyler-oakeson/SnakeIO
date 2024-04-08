using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shared.Controls
{
    public delegate void ControlDelegate(TimeSpan ElapsedTime, float value);
    public delegate void ControlDelegatePosition(TimeSpan ElapsedTime, int x, int y);

    public class ControlManager
    {
        private Dictionary<ControlContext, Control> controls { get; set; } = new Dictionary<ControlContext, Control>();
        private DataManager dataManager;

        public ControlManager(DataManager dm)
        {
            this.dataManager = dm;
            controls = dm.Load<Dictionary<ControlContext, Control>>(controls);
            if (controls.Values.Count <= 0)
            {
                InitializeControls();
            }
        }

        public void InitializeControls()
        {
            controls.Add(ControlContext.MenuUp, new Control(ControlContext.MenuUp, Keys.W, true));
            controls.Add(ControlContext.MenuDown, new Control(ControlContext.MenuDown, Keys.S, true));
            controls.Add(ControlContext.Confirm, new Control(ControlContext.Confirm, Keys.Enter, true));
            controls.Add(ControlContext.MoveUp, new Control(ControlContext.MoveUp, Keys.W, false));
            controls.Add(ControlContext.MoveDown, new Control(ControlContext.MoveDown, Keys.S, false));
            controls.Add(ControlContext.MoveLeft, new Control(ControlContext.MoveLeft, Keys.A, false));
            controls.Add(ControlContext.MoveRight, new Control(ControlContext.MoveRight, Keys.D, false));
            controls.Add(ControlContext.MouseUp, new Control(ControlContext.MouseUp, mouseEvent: MouseEvent.MouseUp));
            controls.Add(ControlContext.MouseDown, new Control(ControlContext.MouseDown, mouseEvent: MouseEvent.MouseDown));
            controls.Add(ControlContext.MouseMove, new Control(ControlContext.MouseMove, mouseEvent: MouseEvent.MouseMove));
            controls.Add(ControlContext.MouseClick, new Control(ControlContext.MouseClick, mouseEvent: MouseEvent.MouseClick));
            SaveKeys();
        }

        /// <summary>
        /// Changes the key used for a registered control and changes what key references the delegate associated with it.
        /// <summary>
        public void ChangeKey(ControlContext cc, Keys key)
        {
            controls[cc].key = key;
            SaveKeys();
        }

        /// <summary>
        /// Returns a key based on the scene and the control context provided.
        /// <summary>
        public Control GetControl(ControlContext cc)
        {
            return controls[cc];
        }

        /// <summary>
        /// Saves the registered controls to file.
        /// <summary>
        public void SaveKeys()
        {
            dataManager.Save<Dictionary<ControlContext, Control>>(controls);
        }
    }
}
