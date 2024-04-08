using Microsoft.Xna.Framework.Audio;

namespace Shared.Components
{
    /// <summary>
    /// This component is responsible for containing soundeffect data of
    /// entites with audio features.
    /// </summary>
    public class Audible : Component
    {
        private SoundEffect sound { get; set; }
        public SoundEffect Sound 
        {
            get { return sound; }
            set { sound = value; }
        }
        private bool play { get; set; }
        public bool Play 
        { 
            get { return play; }
            set { play = value; }
        }

        public Audible(SoundEffect sound)
        {
            this.sound = sound;
            this.Play = false;
        }

        // This will be handled in CreateEntity. These are just used as flags
        public override void Serialize(ref List<byte> data)
        {
        }
    }
}
