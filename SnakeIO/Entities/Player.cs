using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Entities
{
    public class Player
    {
        public static Entity Create(Texture2D texture, SoundEffect sound, Controls.ControlManager cm, Scenes.SceneContext sc, Vector2 pos)
        {
            Entity player = new Entity();

            int radius = texture.Width >= texture.Height ? texture.Width / 2 : texture.Height / 2;

            player.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            player.Add(new Components.Renderable(texture, Color.Red, Color.Black));
            player.Add(new Components.Positionable(pos));
            player.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0)));
            player.Add(new Components.Audible(sound));
            Components.Movable movable = player.GetComponent<Components.Movable>();
            player.Add(new Components.KeyboardControllable(
                        cm,
                        new (Controls.Control, Controls.ControlDelegate)[4]
                        {
                        (new Controls.Control(sc, Controls.ControlContext.MoveUp, Keys.W, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.Velocity += new Vector2(0, -.2f);
                         })),
                        (new Controls.Control(sc, Controls.ControlContext.MoveDown, Keys.S, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.Velocity += new Vector2(0, .2f);
                         })),
                        (new Controls.Control(sc, Controls.ControlContext.MoveRight, Keys.D, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.Velocity += new Vector2(.2f, 0);
                         })),
                        (new Controls.Control(sc, Controls.ControlContext.MoveLeft, Keys.A, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.Velocity += new Vector2(-.2f, 0);
                         })),
                        }));
            
            //Remove if statement for mouse controls. We will want to check what the user selects in the real game
            if (false) { 
                player.Add(new Components.MouseControllable(
                            cm,
                            new (Controls.Control, Controls.ControlDelegatePosition)[1]
                            {
                            (new Controls.Control(sc, Controls.ControlContext.MoveTowards, Keys.None, false),
                             new Controls.ControlDelegatePosition((GameTime gameTime, int x, int y) =>
                                 {
                                 Vector2 pos = player.GetComponent<Components.Positionable>().Pos;
                                 Vector2 dir = new Vector2(x, y) - pos;
                                 dir.Normalize();
                                 movable.Velocity += dir * .2f; //direction * by speed
                                 })),
                            }));
            }
            return player;
        }
    }
}
