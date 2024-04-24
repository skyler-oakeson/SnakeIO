using Microsoft.Xna.Framework;

namespace Shared.Components
{
    public class ParticleComponent : Component
    {
        public ParticleType type { get; set; }
        public bool shouldCreate { get; set; }
        public Vector2 center { get; set; }
        public Vector2 size { get; set; }
        public float rotation { get; set; }
        public float speed { get; set; }
        public TimeSpan lifetime { get; set; }
        public TimeSpan alive { get; set; } = TimeSpan.Zero;
        public Vector2 direction { get; set; }

        public ParticleComponent(ParticleType type, Vector2 center, Vector2 direction, float speed, Vector2 size, TimeSpan lifetime, bool shouldCreate = false)
        {
            this.type = type;
            this.center = center;
            this.speed = speed;
            this.size = size;
            this.lifetime = lifetime;
            this.direction = direction;
            this.shouldCreate = shouldCreate;
        }

        public bool Update(TimeSpan elapsedTime)
        {
            alive += elapsedTime;
            float newX = (float)(elapsedTime.TotalMilliseconds * speed * direction.X);
            float newY = (float)(elapsedTime.TotalMilliseconds * speed * direction.Y);
            center = new Vector2(newX, newY);
            rotation += (speed / .5f);
            return alive < lifetime;
        }

        public override void Serialize(ref List<byte> data)
        {
            data.AddRange(BitConverter.GetBytes((UInt16) type));
            data.AddRange(BitConverter.GetBytes(center.X));
            data.AddRange(BitConverter.GetBytes(center.Y));
            data.AddRange(BitConverter.GetBytes(direction.X));
            data.AddRange(BitConverter.GetBytes(direction.Y));
            data.AddRange(BitConverter.GetBytes(speed));
            data.AddRange(BitConverter.GetBytes(size.X));
            data.AddRange(BitConverter.GetBytes(size.Y));
            data.AddRange(BitConverter.GetBytes((int) lifetime.TotalMilliseconds));
            data.AddRange(BitConverter.GetBytes(shouldCreate));
        }

        public enum ParticleType : UInt16
        {
            FoodParticle,
            PlayerDeathParticle,
            EnemyDeathParticle
        }
    }
}
