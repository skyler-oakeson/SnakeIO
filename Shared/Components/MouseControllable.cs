using System.Collections.Generic;
using Shared.Controls;
namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for keeping mouse control data of
    /// entites that are controllable using a keyboard.
    /// </summary>
    public class MouseControllable : Component
    {
        public Dictionary<ControlContext, ControlDelegatePosition> controls = new Dictionary<ControlContext, ControlDelegatePosition>();
        public bool enable;

        public MouseControllable(
                bool enable,
                Dictionary<ControlContext, ControlDelegatePosition> controls 
                )
        {
            this.controls = controls;
            this.enable = enable;
        }

        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
