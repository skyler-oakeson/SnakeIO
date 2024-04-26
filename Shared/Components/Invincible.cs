using System.Text;

namespace Shared.Components
{
    public class Invincible : Component
    {
        public TimeSpan time { get; set; }
        public float opacity { get; set; } = 1;
        private bool decreaseOpacity { get; set; } = false;

        public Invincible(int time)
        {
            this.time = TimeSpan.FromMilliseconds(time);
        }


        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes((int)time.TotalMilliseconds));
        }

        public void UpdateOpacity()
        {
            Console.WriteLine(opacity);
            if (decreaseOpacity)
            {
                opacity -= .01f;
            }
            else
            {
                opacity += .01f;
            }
            if (opacity > 1f)
            {
                decreaseOpacity = true;
            }
            else if (opacity < .5)
            {
                decreaseOpacity = false;
            }
        }
    }
}

