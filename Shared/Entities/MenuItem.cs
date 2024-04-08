using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Shared.Entities
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
                Shared.Components.LinkPosition linkPos,
                Shared.Controls.ControlManager cm)
        {
            Entity menuItem = new Entity();
            menuItem.Add(new Shared.Components.Renderable<SpriteFont>(font, Color.Orange, Color.Black, value.ToString()));
            menuItem.Add(new Shared.Components.Positionable(pos));
            menuItem.Add(new Shared.Components.Audible(sound));
            menuItem.Add(new Shared.Components.Selectable<T>(selected, value));
            menuItem.Add(new Shared.Components.Linkable(menu, linkPos));

            Shared.Components.Renderable<SpriteFont> renderable = menuItem.GetComponent<Shared.Components.Renderable<SpriteFont>>();
            Shared.Components.Selectable<T> selectable = menuItem.GetComponent<Shared.Components.Selectable<T>>();

            Shared.Components.Linkable link = menuItem.GetComponent<Shared.Components.Linkable>();
            Shared.Components.Audible audio = menuItem.GetComponent<Shared.Components.Audible>();

            menuItem.Add(new Shared.Components.KeyboardControllable(
                        selected,
                        cm,
                        new (Shared.Controls.ControlContext, Shared.Controls.ControlDelegate)[3]
                        {
                        (Shared.Controls.ControlContext.MenuUp,
                         new Shared.Controls.ControlDelegate((GameTime gameTime, float value) =>
                             {
                             link.prevEntity.GetComponent<Components.Selectable<T>>().selected = true;
                             selectable.selected = false;
                             })),
                        (Shared.Controls.ControlContext.MenuDown,
                         new Shared.Controls.ControlDelegate((GameTime gameTime, float value) =>
                             {
                             link.nextEntity.GetComponent<Components.Selectable<T>>().selected = true;
                             selectable.selected = false;
                             })),
                        (Shared.Controls.ControlContext.Confirm,
                         new Shared.Controls.ControlDelegate((GameTime gameTime, float value) =>
                             {
                             selectable.interacted = true;
                             })),
                        }));

            // Menu is a options menu item
            if (typeof(T) == typeof(Shared.Controls.Control))
            {
                Shared.Controls.Control con = (Shared.Controls.Control)Convert.ChangeType(selectable.value, typeof(Shared.Controls.Control));
                renderable.label = $"{con.controlContext}: {con.key}";

                // Change key function
                selectable.selectableDelegate = new Shared.Components.SelectableDelegate(() =>
                {
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
