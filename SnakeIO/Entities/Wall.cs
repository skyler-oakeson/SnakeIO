using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Wall 
    {
        public static Entity Create(Texture2D texture, Vector2 pos)
        {
            Entity wall = new Entity();
            int radius = texture.Width >= texture.Height ? texture.Height/2 : texture.Width/2;

            // wall.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            wall.Add(new Components.Renderable(texture, Color.White, Color.Black));
            wall.Add(new Components.Positionable(pos));
            wall.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            Components.Movable entityMov = wall.GetComponent<Components.Movable>();
            Components.Positionable entityPos = wall.GetComponent<Components.Positionable>();
            wall.Add(new Components.Linkable("player", 
                        Components.LinkPosition.Body,
                        new Components.LinkDelegate((Entities.Entity linked) => 
                            {
                                Components.Positionable linkedPos = linked.GetComponent<Components.Positionable>();
                                entityMov.velocity = (entityMov.velocity - linkedPos.pos);
                                entityMov.velocity.Normalize();
                            })));

            return wall;
        }
    }
}
