using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Components
{
    class Audible : Component
    {
        private SoundEffect sound { get; set; }
        public SoundEffect Sound 
        {
            get { return sound; }
            set { sound = value; }
        }

        public Audible(SoundEffect sound)
        {
            this.sound = sound;
        }
    }
}
