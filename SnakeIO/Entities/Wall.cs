using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Wall 
    {
        public static Entity Create(Texture2D texture, Vector2 pos, Color color)
        {
            Entity wall = new Entity();
            int radius = texture.Width >= texture.Height ? texture.Height/2 : texture.Width/2;

            // wall.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            wall.Add(new Components.Renderable(texture, color, Color.Black));
            wall.Add(new Components.Positionable(pos));
            wall.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            wall.Add(new Components.Linkable("player", 
                        Components.LinkPosition.Body,
                        new Components.LinkDelegate((Entities.Entity e1, Entities.Entity e2) => 
                            {
                                //Linkee
                                Components.Positionable e1Pos = e1.GetComponent<Components.Positionable>();
                                Components.Movable e1Mov = e1.GetComponent<Components.Movable>();

                                // Linked
                                Components.Positionable e2Pos = e2.GetComponent<Components.Positionable>();
                                Components.Movable e2Mov = e2.GetComponent<Components.Movable>();

                                // Vector2 dir = e2Pos.pos - e1Pos.pos;
                                // e1Mov.velocity = dir * .01f;
                                Vector2 offset = e2Mov.velocity;
                                offset = offset * radius;
                                e1Pos.prevPos = e1Pos.pos;
                                e1Pos.pos = e2Pos.prevPos + offset;
                            })));
            return wall;
        }
    }
}
