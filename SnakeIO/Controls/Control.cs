using Microsoft.Xna.Framework.Input;
using System.Runtime.Serialization;

namespace Controls
{
    [DataContract(Name = "Control")]
    public class Control
    {
        [DataMember()]
        public ControlContext controlContext { get; private set; }
        [DataMember()]
        public Keys? key { get; set; }
        [DataMember()]
        public MouseEvent? mouseEvent { get; set; }
        [DataMember()]
        public bool? keyPressOnly { get; set; }

        public Control(ControlContext controlContext, Keys? key = null, bool? keyPressOnly = null, MouseEvent? mouseEvent = null)
        {
            this.controlContext = controlContext; 
            this.key = key;
            this.keyPressOnly = keyPressOnly;
            this.mouseEvent = mouseEvent;
        }
    }
}
