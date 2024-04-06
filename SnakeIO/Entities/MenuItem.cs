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
        public static Entity Create(SpriteFont font, Scenes.SceneContext sc, string menu, bool highlighted, Vector2 pos, SoundEffect sound, Components.LinkPosition linkPos, Controls.ControlManager cm)
        {
            Entity menuItem = new Entity();
            menuItem.Add(new Components.Linkable(menu, linkPos));
            menuItem.Add(new Components.Renderable<SpriteFont>(font, Color.Black, Color.Orange, sc.ToString()));
            menuItem.Add(new Components.Positionable(pos));
            menuItem.Add(new Components.Audible(sound));
            menuItem.Add(new Components.Selectable(highlighted, new Components.SelectionDelegate(()=>{ return sc; })));

            Components.Selectable selectable = menuItem.GetComponent<Components.Selectable>();
            Components.Renderable<SpriteFont> renderable = menuItem.GetComponent<Components.Renderable<SpriteFont>>();
            Components.Linkable link = menuItem.GetComponent<Components.Linkable>();
            Components.Audible audio = menuItem.GetComponent<Components.Audible>();
            menuItem.Add(new Components.KeyboardControllable(
                        cm,
                        new (ControlContext, ControlDelegate)[3]
                        {
                        (ControlContext.MenuUp,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             if (link.prevEntity != null)
                             {
                                 selectable.highlighted = false;
                                 link.prevEntity.GetComponent<Components.Selectable>().highlighted = true;
                             }
                             })),
                        (ControlContext.MenuDown,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             if (link.nextEntity != null)
                             {
                                 selectable.highlighted = false;
                                 link.nextEntity.GetComponent<Components.Selectable>().highlighted = true;
                                 Console.WriteLine("Down");
                             }
                             })),
                        (ControlContext.Confirm,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             selectable.selected = true;
                             })),
                        }));

            return menuItem;
        }
    }
}
