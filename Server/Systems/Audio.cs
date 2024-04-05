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
    public class Audio : Shared.Systems.System
    {
        private Queue<SoundEffect> Sounds;
        public Audio()
            : base(
                    typeof(Shared.Components.Audible)
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

        private void PlaySound(Shared.Entities.Entity entity)
        {
            Shared.Components.Audible audible = entity.GetComponent<Shared.Components.Audible>();
            if (audible.Play)
            {
                audible.Sound.Play();
                audible.Play = false;
            }
        }
    }
}
