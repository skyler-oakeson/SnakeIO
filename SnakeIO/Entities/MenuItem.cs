using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Controls;

namespace Entities
{
    public class MenuItem
    {
        public static Entity Create(SpriteFont font, Scenes.SceneContext sc, string menu, bool selected, Vector2 pos, SoundEffect sound, Components.LinkPosition linkPos, Controls.ControlManager cm)
        {
            Entity menuItem = new Entity();
            menuItem.Add(new Components.Renderable<SpriteFont>(font, Color.Black, Color.Orange, sc.ToString()));
            menuItem.Add(new Components.Positionable(pos));
            menuItem.Add(new Components.Audible(sound));
            menuItem.Add(new Components.Selectable(selected, new Components.SelectionDelegate(() => { Console.WriteLine(sc); return sc;})));
            menuItem.Add(new Components.Linkable(menu, linkPos));

            Components.Selectable selectable = menuItem.GetComponent<Components.Selectable>();
            Components.Linkable link = menuItem.GetComponent<Components.Linkable>();
            Components.Audible audio = menuItem.GetComponent<Components.Audible>();

            menuItem.Add(new Components.KeyboardControllable(
                        selected,
                        cm,
                        new (ControlContext, ControlDelegate)[3]
                        {
                        (ControlContext.MenuUp,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             link.prevEntity.GetComponent<Components.Selectable>().selected = true;
                             selectable.selected = false;
                             })),
                        (ControlContext.MenuDown,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             link.nextEntity.GetComponent<Components.Selectable>().selected = true;
                             selectable.selected = false;
                             })),
                        (ControlContext.Confirm,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             selectable.interacted = true;
                             })),
                        }));

            return menuItem;
        }
    }
}
