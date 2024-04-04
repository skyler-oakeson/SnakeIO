using Microsoft.Xna.Framework.Audio;

namespace Components
{
    /// <summary>
    /// This component is responsible for containing soundeffect data of
    /// entites with audio features.
    /// </summary>
    class Audible : Component
    {
        public SoundEffect sound { get; set; }
        public bool play { get; set; }

        public Audible(SoundEffect sound)
        {
            this.sound = sound;
            this.play = false;
        }
    }
}
