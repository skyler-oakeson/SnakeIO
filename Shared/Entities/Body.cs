using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Shared.Entities
{
    public class Body 
    {
        public static Entity Create(string texture, Color color, string sound, Shared.Controls.ControlManager cm, Rectangle rectangle, string chain = null)
        {
            Entity body = new Entity();

            body.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Black, rectangle));
            //TODO: research if this will actually work
            // body.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            body.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            body.Add(new Shared.Components.Movable(new Vector2(0, 0)));
            // body.Add(new Components.Audible(sound));
            Shared.Components.Movable movable = body.GetComponent<Shared.Components.Movable>();

            if (chain != null)
            {
                body.Add(new Components.Linkable(chain, 
                            Components.LinkPosition.Body,
                            new Components.LinkDelegate((Entities.Entity root) => 
                                {
                                Components.Linkable rootLink = root.GetComponent<Components.Linkable>();
                                Components.Positionable rootPos = root.GetComponent<Components.Positionable>();
                                Components.Movable rootMov = root.GetComponent<Components.Movable>();
                                Components.Positionable prevPos = rootLink.prevEntity.GetComponent<Components.Positionable>();
                                Components.Movable prevMov = rootLink.prevEntity.GetComponent<Components.Movable>();

                                // Vector2 offset = prevMov.velocity;
                                // offset = offset * radius;
                                // rootPos.prevPos = rootPos.pos;
                                // rootPos.pos = prevPos.prevPos;
                                rootPos.prevPos = rootPos.pos;
                                rootPos.pos = prevPos.prevPos;
                                })));
            }
            return body;
        }
    }
}
