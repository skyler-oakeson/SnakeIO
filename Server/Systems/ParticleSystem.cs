using Microsoft.Xna.Framework;
using System.Threading;

namespace Systems
{
    public class ParticleSystem : Shared.Systems.System
    {
        private int sizeMean { get; set; }
        private int sizeStdDev { get; set; }
        private float speedMean { get; set; }
        private float speedStdDev { get; set; }
        private float lifetimeMean { get; set; }
        private float lifetimeStdDev { get; set; }
        private Shared.MyRandom rand = new Shared.MyRandom();

        public ParticleSystem()
            : base(
                    typeof(Shared.Components.ParticleComponent),
                    typeof(Shared.Components.Appearance)
                    )
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
        }

        private Shared.Entities.Particle CreateParticle(Vector2 direction, Shared.Entities.Entity entity)
        {
            float size = (float)rand.nextGaussian(sizeMean, sizeStdDev);
            Shared.Components.ParticleComponent pComponent = entity.GetComponent<Shared.Components.ParticleComponent>();
            Shared.Components.Appearance appearance = entity.GetComponent<Shared.Components.Appearance>();
            Shared.Entities.Particle p = new Shared.Entities.Particle.Create(
                    appearance.texturePath,
                    appearance.rectangle,
                    appearance.color,
                    pComponent.type,
                    pComponent.center,
                    direction,
                    (float)rand.nextGaussian(speedMean, speedStdDev),
                    new Vector2(size, size),
                    new TimeSpan(0, 0, 0, 0, (int)rand.nextGaussian(lifetimeMean, lifetimeStdDev))
                    );
            return p;
        }

        private void PlayerDeath()
        {

        }

        private void EnemyDeath()
        {

        }

        private void EatFood()
        {

        }
    }
}
