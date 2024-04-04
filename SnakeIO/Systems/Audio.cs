using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling playing soundeffects of any
    /// entity with an Audible component.
    /// </summary>
    public class Audio : System
    {
        private Queue<SoundEffect> Sounds;
        public Audio()
            : base(
                    typeof(Components.Audible)
                    )
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                PlaySound(entity);
            }
        }

        private void PlaySound(Entities.Entity entity)
        {
            Components.Audible audible = entity.GetComponent<Components.Audible>();
            if (audible.play)
            {
                audible.sound.Play();
                audible.play = false;
            }
        }
    }
}
