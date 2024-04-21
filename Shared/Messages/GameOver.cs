
namespace Shared.Messages
{
    public class GameOver : Message
    {
        public GameOver() : base(Type.GameOver)
        {

        }

        /// <summary>
        /// Just knowing this message is sent is enough
        /// </summary>
        public override byte[] serialize()
        {
            return base.serialize();
        }

        public override int parse(byte[] data)
        {
            return base.parse(data);
        }
    }
}
