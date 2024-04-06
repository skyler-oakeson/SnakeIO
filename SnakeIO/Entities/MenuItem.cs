using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Controls;
using Components;

namespace Entities
{
    public class MenuItem
    {
        public static Entity Create(SpriteFont font, string label, bool selected, Vector2 pos, SoundEffect sound, Components.LinkPosition linkPos, Controls.ControlManager cm)
        {
            Entity menuItem = new Entity();
            menuItem.Add(new Selectable(selected));
            menuItem.Add(new Linkable("menu", linkPos));
            menuItem.Add(new Renderable<SpriteFont>(font, Color.Orange, Color.White, label));
            menuItem.Add(new Positionable(pos));
            menuItem.Add(new Audible(sound));
            Selectable selectable = menuItem.GetComponent<Selectable>();
            Linkable link = menuItem.GetComponent<Linkable>();
            Audible audio = menuItem.GetComponent<Audible>();
            menuItem.Add(new KeyboardControllable(
                        cm,
                        new (ControlContext, ControlDelegate)[3]
                        {
                        (ControlContext.MenuUp,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                                 Console.WriteLine("UP");
                                 selectable.selected = false;
                                 link.prevEntity.GetComponent<Components.Selectable>().selected = true;
                             })),
                        (ControlContext.MenuDown,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                                 Console.WriteLine("Down");
                                 selectable.selected = false;
                                 link.nextEntity.GetComponent<Components.Selectable>().selected = true;
                             })),
                        (ControlContext.Confirm,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                                 Console.WriteLine("Enter");
                             })),
                        }));

            return menuItem;
        }
    }
}
