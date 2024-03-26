using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Entities
{
    public class Player
    {
        public static Entity Create(Texture2D texture, Controls.ControlManager cm, Scenes.SceneContext sc, Vector2 pos)
        {
            Entity player = new Entity();

            player.Add(new Components.Renderable(texture, Color.White, Color.Black));
            player.Add(new Components.Positionable(pos));
            player.Add(new Components.Movable(new Vector2(0, 0), new Vector2(0, 0), DateTime.Now.TimeOfDay));
            Components.Movable movable = player.GetComponent<Components.Movable>();
            player.Add(new Components.KeyboardControllable(
                        cm,
                        new (Controls.Control, Controls.ControlDelegate)[4]
                        {
                        (new Controls.Control(sc, Controls.ControlContext.MoveUp, Keys.W, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.facing = new Vector2(0, 1);
                            movable.velocity = new Vector2(.03f, .03f);
                         })),
                        (new Controls.Control(sc, Controls.ControlContext.MoveDown, Keys.S, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.facing = new Vector2(0, -1);
                            movable.velocity = new Vector2(.03f, .03f);
                         })),
                        (new Controls.Control(sc, Controls.ControlContext.MoveRight, Keys.D, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.facing = new Vector2(-1, 0);
                            movable.velocity = new Vector2(.03f, .03f);
                         })),
                        (new Controls.Control(sc, Controls.ControlContext.MoveLeft, Keys.A, false),
                         new Controls.ControlDelegate((GameTime gameTime, float value) =>
                         {
                            movable.facing = new Vector2(1, 0);
                            movable.velocity = new Vector2(.03f, .03f);
                         })),
                        }));
            return player;
        }
    }
}
