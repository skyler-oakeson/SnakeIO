using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Shared.Entities
{
    public class Player
    {
        public static Entity Create(string texture, Color color, string sound, Shared.Controls.ControlManager cm, Rectangle rectangle, string chain = null)
        {
            Entity player = new Entity();

            if (chain != null)
            {
                player.Add(new Shared.Components.Linkable(chain, Shared.Components.LinkPosition.Head));
            }

            player.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Black, rectangle));
            // player.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            // player.Add(new Components.Renderable(texture, "Images/player", color, Color.Black));
            player.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y)));
            player.Add(new Shared.Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            // player.Add(new Components.Audible(sound));
            Shared.Components.Movable movable = player.GetComponent<Shared.Components.Movable>();
            player.Add(new Shared.Components.KeyboardControllable(
                true,
                Shared.Controls.ControlableEntity.Player,
                cm,
                new (Shared.Controls.ControlContext, Shared.Controls.ControlDelegate)[4]
                {
                (Shared.Controls.ControlContext.MoveUp,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, -.2f);
                     })),
                (Shared.Controls.ControlContext.MoveDown,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, .2f);
                     })),
                (Shared.Controls.ControlContext.MoveRight,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(.2f, 0);
                     })),
                (Shared.Controls.ControlContext.MoveLeft,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(-.2f, 0);
                     })),
                }));
            //Remove if statement for mouse controls. We will want to check what the user selects in the real game
            if (false)
            {
                player.Add(new Shared.Components.MouseControllable(
                            cm,
                            new (Shared.Controls.ControlContext, Shared.Controls.ControlDelegatePosition)[1]
                            {
                            (Shared.Controls.ControlContext.MouseMove,
                             new Shared.Controls.ControlDelegatePosition((TimeSpan elapsedTime, int x, int y) =>
                                 {
                                 Vector2 pos = player.GetComponent<Shared.Components.Positionable>().pos;
                                 Vector2 dir = new Vector2(x, y) - pos;
                                 dir.Normalize();
                                 movable.velocity += dir * .2f; //direction * by speed
                                 })),
                            }));
            }
            return player;
        }
    }
}
