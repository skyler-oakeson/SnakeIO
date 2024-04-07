using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Controls;

namespace Entities
{
    public class MenuItem<T>
    {
        public static Entity Create(
                SpriteFont font,
                T value,
                string menu,
                bool selected,
                Vector2 pos,
                SoundEffect sound,
                Components.LinkPosition linkPos,
                Controls.ControlManager cm)
        {
            Entity menuItem = new Entity();
            menuItem.Add(new Components.Renderable<SpriteFont>(font, Color.Orange, Color.Black, value.ToString()));
            menuItem.Add(new Components.Positionable(pos));
            menuItem.Add(new Components.Audible(sound));
            menuItem.Add(new Components.Selectable<T>(selected, value));
            menuItem.Add(new Components.Linkable(menu, linkPos));

            Components.Renderable<SpriteFont> renderable = menuItem.GetComponent<Components.Renderable<SpriteFont>>();
            Components.Selectable<T> selectable = menuItem.GetComponent<Components.Selectable<T>>();

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
                             link.prevEntity.GetComponent<Components.Selectable<T>>().selected = true;
                             selectable.selected = false;
                             })),
                        (ControlContext.MenuDown,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             link.nextEntity.GetComponent<Components.Selectable<T>>().selected = true;
                             selectable.selected = false;
                             })),
                        (ControlContext.Confirm,
                         new ControlDelegate((GameTime gameTime, float value) =>
                             {
                             selectable.interacted = true;
                             })),
                        }));

            // Menu is a options menu item
            if (typeof(T) == typeof(Control))
            {
                Control con = (Control)Convert.ChangeType(selectable.value, typeof(Control));
                renderable.label = $"{con.controlContext}: {con.key}";
                
                // Change key function
                selectable.selectableDelegate = new Components.SelectableDelegate(() => {
                        Keys[] newKey = Keyboard.GetState().GetPressedKeys();
                        if (newKey.Length > 0)
                        {
                            if (newKey[0] != Keys.Enter)
                            {
                                cm.ChangeKey(con.controlContext, newKey[0]);
                                renderable.label = $"{con.controlContext}: {newKey[0]}";
                                return true;
                            }
                        }
                        return false;
                        });
            }

            return menuItem;
        }
    }
}
