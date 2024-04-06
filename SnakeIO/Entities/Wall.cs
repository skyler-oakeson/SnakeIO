using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Wall 
    {
        public static Entity Create(Texture2D texture, string name, Vector2 pos, Color color)
        {
            Entity wall = new Entity();
            int radius = texture.Width >= texture.Height ? texture.Height/2 : texture.Width/2;

            // wall.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            wall.Add(new Components.Renderable<Texture2D>(texture, color, Color.Black));
            wall.Add(new Components.Positionable(pos));
            wall.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            wall.Add(new Components.Linkable(name, 
                        Components.LinkPosition.Body,
                        new Components.LinkDelegate((Entities.Entity root) => 
                            {
                                Components.Linkable rootLink = root.GetComponent<Components.Linkable>();
                                Components.Positionable rootPos = root.GetComponent<Components.Positionable>();
                                Components.Movable rootMov = root.GetComponent<Components.Movable>();
                                Components.Positionable prevPos = rootLink.prevEntity.GetComponent<Components.Positionable>();
                                Components.Movable prevMov = rootLink.prevEntity.GetComponent<Components.Movable>();

                                Vector2 offset = prevMov.velocity;
                                offset = offset * radius;
                                rootPos.prevPos = rootPos.pos;
                                rootPos.pos = prevPos.prevPos - offset;
                            })));
            return wall;
        }
    }
}
