namespace Shared.Parsers
{
    public class AnimatableParser : Parser
    {
        private AnimatableMessage message { get; set; }
        public AnimatableMessage Message 
        {
            get { return message; }
            set { message = value; }
        }
        //Parse everything but Texture2D
        public override void Parse(ref byte[] data, ref int offset)
        {
        }

        public AnimatableMessage GetMessage() 
        {
            return Message;
        }

        public struct AnimatableMessage
        {
            public int[] spriteTime;
            public int subImageWidth;
            public int subImageIndex;
            public TimeSpan timeSinceLastFrame;
        }
    }
}
