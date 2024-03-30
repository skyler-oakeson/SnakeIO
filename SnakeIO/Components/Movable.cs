using Microsoft.Xna.Framework;

namespace Components
{
    /// <summary>
    /// This component is responsible for keeping track of the state of movable entities.
    /// </summary>
    public class Movable : Component
    {
        private Vector2 rotation { get; set; }
        public Vector2 Rotation 
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private Vector2 velocity { get; set; }
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Movable(Vector2 rotation, Vector2 velocity)
        {
            this.rotation = rotation;
            this.velocity = velocity;
        }

    }
}
