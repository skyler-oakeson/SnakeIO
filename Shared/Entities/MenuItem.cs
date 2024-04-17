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
                SoundEffect sound,
                Shared.Components.LinkPosition linkPos,
                Shared.Controls.ControlManager cm,
                Rectangle rectangle)
        {
            Entity menuItem = new Entity();
            menuItem.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), Color.Orange, Color.Black, rectangle));
            menuItem.Add(new Shared.Components.Readable(font, "Fonts/Micro5", value.ToString(), Color.Orange, Color.Black, rectangle));
            menuItem.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            menuItem.Add(new Shared.Components.Audible(sound));
            menuItem.Add(new Shared.Components.Selectable<T>(selected, value));
            menuItem.Add(new Shared.Components.Linkable(menu, linkPos));

            Shared.Components.Readable readable = menuItem.GetComponent<Shared.Components.Readable>();
            Shared.Components.Audible audio = menuItem.GetComponent<Shared.Components.Audible>();
            Shared.Components.Selectable<T> selectable = menuItem.GetComponent<Shared.Components.Selectable<T>>();
            Shared.Components.Linkable link = menuItem.GetComponent<Shared.Components.Linkable>();

            menuItem.Add(new Shared.Components.KeyboardControllable(selected, typeof(Shared.Entities.MenuItem<T>), MenuControls));
            // Menu is a options menu item
            if (typeof(T) == typeof(Shared.Controls.Control))
            {
                Shared.Controls.Control con = (Shared.Controls.Control)Convert.ChangeType(selectable.value, typeof(Shared.Controls.Control));
                readable.text = $"{con.controlContext}: {con.key}";

                // Change key function
                selectable.selectableDelegate = new Shared.Components.SelectableDelegate(() =>
                {
                    Keys[] newKey = Keyboard.GetState().GetPressedKeys();
                    if (newKey.Length > 0)
                    {
                        if (newKey[0] != Keys.Enter)
                        {
                            cm.ChangeKey(con.controlContext, newKey[0]);
                            readable.text = $"{con.controlContext}: {newKey[0]}";
                            return true;
                        }
                    }
                    return false;
                });
            }

            return menuItem;
        }

        public static Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate> MenuControls = new Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate>
        {
            {
                Shared.Controls.ControlContext.MenuUp,
                    new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                            {
                            Shared.Components.Selectable<T> selectable = entity.GetComponent<Shared.Components.Selectable<T>>();
                            Shared.Components.Linkable link = entity.GetComponent<Shared.Components.Linkable>();
                            if (link.prevEntity != null)
                            {
                            link.prevEntity.GetComponent<Components.Selectable<T>>().selected = true;
                            selectable.selected = false;
                            }
                            })
            },
            {
                Shared.Controls.ControlContext.MenuDown,
                    new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                            {
                            Shared.Components.Selectable<T> selectable = entity.GetComponent<Shared.Components.Selectable<T>>();
                            Shared.Components.Linkable link = entity.GetComponent<Shared.Components.Linkable>();
                            if (link.prevEntity != null)
                            {
                            link.nextEntity.GetComponent<Components.Selectable<T>>().selected = true;
                            selectable.selected = false;
                            }
                            })
            },
            {
                Shared.Controls.ControlContext.Confirm,
                new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                        {
                        Shared.Components.Selectable<T> selectable = entity.GetComponent<Shared.Components.Selectable<T>>();
                        Shared.Components.Linkable link = entity.GetComponent<Shared.Components.Linkable>();
                        selectable.interacted = true;
                        })
            }
        };
}
}
