using Microsoft.Xna.Framework.Audio;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for containing soundeffect data of
    /// entites with audio features.
    /// </summary>
    public class Audible : Component
    {
        public SoundEffect sound { get; set; }
        public bool play { get; set; }

        public Audible(SoundEffect sound)
        {
            this.sound = sound;
            this.play = false;
        }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
