using System.Text;

namespace Shared.Components
{
    public class Invincible : Component
    {
        public TimeSpan time { get; set; }
        public float opacity { get; set; } = 0;

        public Invincible(int time)
        {
            this.time = TimeSpan.FromMilliseconds(time);
        }


        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes((int)time.TotalMilliseconds));
        }
    }
}

