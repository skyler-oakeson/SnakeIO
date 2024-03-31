using Microsoft.Xna.Framework.Audio;

namespace Components
{
    /// <summary>
    /// This component is responsible for containing soundeffect data of
    /// entites with audio features.
    /// </summary>
    class Audible : Component
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
    }
}
