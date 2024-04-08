using System.Text;

namespace Shared.Components
{
    public class Sound : Component
    {
        public string soundPath { get; private set; }

        public Sound(string soundPath)
        {
            this.soundPath = soundPath;
        }


        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes(soundPath.Length));
        }
    }
}

