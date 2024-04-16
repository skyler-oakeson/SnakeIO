using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Shared.Entities
{
    public class Head 
    {
        public static Entity Create(string texture, Color color, string sound, Shared.Controls.ControlManager cm, Rectangle rectangle, string chain = null)
        {
            Entity head = new Entity();

            if (chain != null)
            {
                head.Add(new Shared.Components.Linkable(chain, Shared.Components.LinkPosition.Head));
            }

            head.Add(new Components.Appearance(texture, typeof(Texture2D), color, Color.Black, rectangle));
            //TODO: research if this will actually work
            head.Add(new Components.Camera(new Rectangle(rectangle.X, rectangle.Y, 1500, 1500)));
            // head.Add(new Components.Collidable(new Vector3(pos.X, pos.Y, radius)));
            head.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            head.Add(new Shared.Components.Movable(new Vector2(0, 0)));
            // head.Add(new Components.Audible(sound));
            Shared.Components.Movable movable = head.GetComponent<Shared.Components.Movable>();
            head.Add(new Shared.Components.KeyboardControllable(
                true,
                Shared.Controls.ControlableEntity.Player,
                cm,
                new (Shared.Controls.ControlContext, Shared.Controls.ControlDelegate)[4]
                {
                (Shared.Controls.ControlContext.MoveUp,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, -.1f);
                     })),
                (Shared.Controls.ControlContext.MoveDown,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(0, .1f);
                     })),
                (Shared.Controls.ControlContext.MoveRight,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(.1f, 0);
                     })),
                (Shared.Controls.ControlContext.MoveLeft,
                     new Shared.Controls.ControlDelegate((TimeSpan elapsedTime, float value) =>
                     {
                     movable.velocity += new Vector2(-.1f, 0);
                     })),
                }));
            //Remove if statement for mouse controls. We will want to check what the user selects in the real game
            if (false)
            {
                head.Add(new Shared.Components.MouseControllable(
                            cm,
                            new (Shared.Controls.ControlContext, Shared.Controls.ControlDelegatePosition)[1]
                            {
                            (Shared.Controls.ControlContext.MouseMove,
                             new Shared.Controls.ControlDelegatePosition((TimeSpan elapsedTime, int x, int y) =>
                                 {
                                 Vector2 pos = head.GetComponent<Shared.Components.Positionable>().pos;
                                 Vector2 dir = new Vector2(x, y) - pos;
                                 dir.Normalize();
                                 movable.velocity += dir * .2f; //direction * by speed
                                 })),
                            }));
            }
            return head;
        }
    }
}
