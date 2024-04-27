using Microsoft.Xna.Framework.Audio;

namespace Shared.Components
{
    public class KillCount : Component
    {
        public int count { get; set; }
        public KillCount()
        {
            this.count = 0;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(count));
        }
        public void UpdateCount()
        {
            this.count += 1;
        }
    }
}
