using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Shared.Entities
{
    public class Player
    {
        public static Entity Create(string texture, Color color, string sound, Rectangle rectangle, string chain = null)
        {
            Entity player = new Entity();

            if (chain != null)
            {
                player.Add(new Shared.Components.Linkable(chain, Shared.Components.LinkPosition.Head));
            }
            player.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Black, rectangle));
            player.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            player.Add(new Shared.Components.Movable(new Vector2(0, 0)));
            player.Add(new Shared.Components.KeyboardControllable(true, PlayerKeyboardControls));
            player.Add(new Components.Camera(new Rectangle(rectangle.X, rectangle.Y, 1500, 1500)));
            // player.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            // player.Add(new Components.Audible(sound));

            return player;
        }

        public static Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate> PlayerKeyboardControls = 
            new Dictionary<Controls.ControlContext, Controls.ControlDelegate>
        {
            {
                Shared.Controls.ControlContext.MoveUp, new Shared.Controls.ControlDelegate((Shared.Entities.Entity entity, TimeSpan elapsedTime) =>
                        {
                        Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
                        movable.velocity += new Vector2(0, -.1f);
                        })
            },
            {
                Shared.Controls.ControlContext.MoveDown, new Shared.Controls.ControlDelegate((Shared.Entities.Entity entity, TimeSpan elapsedTime) =>
                        {
                        Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
                        movable.velocity += new Vector2(0, .1f);
                        })
            },
            {
                Shared.Controls.ControlContext.MoveRight, new Shared.Controls.ControlDelegate((Shared.Entities.Entity entity, TimeSpan elapsedTime) =>
                        {
                        Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
                        movable.velocity += new Vector2(.1f, 0);
                        })
            },
            {
                Shared.Controls.ControlContext.MoveLeft, new Shared.Controls.ControlDelegate((Shared.Entities.Entity entity, TimeSpan elapsedTime) =>
                        {
                        Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
                        movable.velocity += new Vector2(-.1f, 0);
                        })
            }
        };

        public static (Shared.Controls.ControlContext, Shared.Controls.ControlDelegatePosition)[] PlayerMouseControls = 
            new (Shared.Controls.ControlContext, Shared.Controls.ControlDelegatePosition)[1]
        {
            (Shared.Controls.ControlContext.MoveLeft, new Shared.Controls.ControlDelegatePosition((Shared.Entities.Entity entity, TimeSpan elapsedTime, int x, int y) =>
            {
                Shared.Components.Movable movable = entity.GetComponent<Shared.Components.Movable>();
                Vector2 pos = entity.GetComponent<Shared.Components.Positionable>().pos;
                Vector2 dir = new Vector2(x, y) - pos;
                dir.Normalize();
                movable.velocity += dir * .2f; //direction * by speed
            }))
        };
    }
}
