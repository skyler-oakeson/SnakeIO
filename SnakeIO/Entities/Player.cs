using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using Controls;

namespace Entities
{
    public class Player
    {
        public static Entity Create(Texture2D texture, string name, Color color, SoundEffect sound, ControlManager cm, Vector2 pos)
        {
            Entity player = new Entity();

            int radius = texture.Width >= texture.Height ? texture.Width / 2 : texture.Height / 2;

            player.Add(new Components.Linkable(name, Components.LinkPosition.Head));
            player.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            player.Add(new Components.Renderable<Texture2D>(texture, color, Color.Black));
            player.Add(new Components.Positionable(pos));
            player.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            player.Add(new Components.Audible(sound));
            Components.Movable movable = player.GetComponent<Components.Movable>();
            player.Add(new Components.KeyboardControllable(
                cm,
                new (ControlContext, Controls.ControlDelegate)[4]
                {
                (ControlContext.MoveUp,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, -.2f);
                     })),
                (ControlContext.MoveDown,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, .2f);
                     })),
                (ControlContext.MoveRight,
                     new Controls.ControlDelegate((GameTime gameTime, float value) =>
                     {
                     movable.velocity += new Vector2(.2f, 0);
                     })),
                (ControlContext.MoveLeft,
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
                            new (ControlContext, ControlDelegatePosition)[1]
                            {
                            (ControlContext.MouseMove,
                             new ControlDelegatePosition((GameTime gameTime, int x, int y) =>
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
