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
            int spriteTimeLength = BitConverter.ToInt32(data, offset);
            offset += sizeof(Int32);
            int[] messageSpriteTime = new int[spriteTimeLength];
            for (int i = 0; i < spriteTimeLength; i++)
            {
                messageSpriteTime[i] = BitConverter.ToInt32(data, offset);
                offset += sizeof(Int32);
            }
            this.message = new AnimatableMessage
            {
                spriteTime = messageSpriteTime,
            };
        }

        public AnimatableMessage GetMessage()
        {
            return Message;
        }

        public struct AnimatableMessage
        {
            public int[] spriteTime;
        }
    }
}
