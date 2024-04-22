using System;
using Microsoft.Xna.Framework;
using System.Text;
namespace Shared.Parsers
{
    public class AppearanceParser : Parser
    {
        private AppearanceMessage message { get; set; }
        public AppearanceMessage Message
        {
            get { return message; }
            set { message = value; }
        }

        public override void Parse(ref byte[] data, ref int offset)
        {
        }

        public AppearanceMessage GetMessage()
        {
            return Message;
        }

        public struct AppearanceMessage
        {
        }
    }
}
