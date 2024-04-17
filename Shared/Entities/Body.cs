using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Shared.Entities
{
    public class Body
    {
        public static Entity Create(string texture, Color color, string sound, Rectangle rectangle, string chain, Shared.Components.LinkPosition linkPos)
        {
            Entity body = new Entity();

            body.Add(new Shared.Components.Linkable(chain, linkPos, BodyLinking));
            body.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Black, rectangle));
            body.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            body.Add(new Shared.Components.Movable(new Vector2(0, 0)));
            // body.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            // body.Add(new Components.Audible(sound));

            return body;
        }

        public static Shared.Components.LinkDelegate BodyLinking = new Components.LinkDelegate((Entities.Entity root) =>
        {
            Components.Linkable rootLink = root.GetComponent<Components.Linkable>();
            Components.Positionable rootPos = root.GetComponent<Components.Positionable>();
            Components.Movable rootMov = root.GetComponent<Components.Movable>();
            Components.Positionable prevPos = rootLink.prevEntity.GetComponent<Components.Positionable>();
            Components.Movable prevMov = rootLink.prevEntity.GetComponent<Components.Movable>();

            Vector2 offset = prevMov.velocity;
            // offset = offset * radius;
            // rootPos.pos = prevPos.prevPos - offset;
            rootPos.UpdatePoistion(prevPos.prevPos);
        });
    }
}
