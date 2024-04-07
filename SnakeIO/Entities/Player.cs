using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Entities
{
    public class Player
    {
        public static Entity Create(Texture2D texture, Color color, SoundEffect sound, Controls.ControlManager cm, Vector2 pos, string chain = null)
        {
            Entity player = new Entity();

            int radius = texture.Width >= texture.Height ? texture.Width / 2 : texture.Height / 2;

            if (chain != null)
            {
                player.Add(new Components.Linkable(chain, Components.LinkPosition.Head));
            }

            player.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            player.Add(new Components.Renderable<Texture2D>(texture, color, Color.Black));
            player.Add(new Components.Positionable(pos));
            player.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            player.Add(new Components.Audible(sound));
            Components.Movable movable = player.GetComponent<Components.Movable>();
            player.Add(new Components.KeyboardControllable(
                true,
                cm,
                new (Controls.ControlContext, Controls.ControlDelegate)[4]
                {
                (Controls.ControlContext.MoveUp,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, -.2f);
                     })),
                (Controls.ControlContext.MoveDown,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, .2f);
                     })),
                (Controls.ControlContext.MoveRight,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(.2f, 0);
                     })),
                (Controls.ControlContext.MoveLeft,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(-.2f, 0);
                     })),
                }));

            //Remove if statement for mouse controls. We will want to check what the user selects in the real game
            if (false)
            {
                player.Add(new Components.MouseControllable(
                            cm,
                            new (Controls.ControlContext, Controls.ControlDelegatePosition)[1]
                            {
                            (Controls.ControlContext.MouseMove,
                             new Controls.ControlDelegatePosition((GameTime gameTime, int x, int y) =>
                                 {
                                 Vector2 pos = player.GetComponent<Components.Positionable>().pos;
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
