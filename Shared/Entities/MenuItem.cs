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
            menuItem.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), Color.Green, Color.Black, rectangle));
            menuItem.Add(new Shared.Components.Readable(font, "Fonts/Micro5", value.ToString(), Color.Green, Color.Black, rectangle));
            menuItem.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            menuItem.Add(new Shared.Components.Audible(sound));
            menuItem.Add(new Shared.Components.Selectable<T>(selected, value));
            menuItem.Add(new Shared.Components.Linkable(menu, linkPos));
            menuItem.Add(new Shared.Components.KeyboardControllable(selected, typeof(Shared.Entities.MenuItem<T>), MenuControls));

            // Menu is a options menu item
            if (typeof(T) == typeof(Shared.Controls.Control))
            {
                Shared.Components.Selectable<T> selectable = menuItem.GetComponent<Shared.Components.Selectable<T>>();
                Shared.Components.Readable readable = menuItem.GetComponent<Shared.Components.Readable>();
                Shared.Controls.Control con = (Shared.Controls.Control)Convert.ChangeType(selectable.value, typeof(Shared.Controls.Control));
                readable.text = $"{con.controlContext}: {con.key}";
                selectable.interactableDelegate =
                new Shared.Components.InteractableDelegate((Shared.Entities.Entity entity) =>
                {
                    Keys[] newKey = Keyboard.GetState().GetPressedKeys();
                    if (newKey.Length > 0)
                    {
                        if (!newKey.Contains(Keys.Enter) && !newKey.Contains(Keys.Escape))
                        {
                            cm.ChangeKey(con.controlContext, newKey[0]);
                            readable.text = $"{con.controlContext}: {newKey[0]}";
                            return true;
                        }
                        else 
                        {
                            return false;
                        }
                    }
                    return false;
                });
            }

            return menuItem;
        }

        public static Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate> MenuControls =
            new Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate>
        {
            {
                Shared.Controls.ControlContext.MenuUp,
                    new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                    {
                    Shared.Components.Selectable<T> selectable = entity.GetComponent<Shared.Components.Selectable<T>>();
                    Shared.Components.Linkable link = entity.GetComponent<Shared.Components.Linkable>();
                    Shared.Components.Audible sound = entity.GetComponent<Shared.Components.Audible>();
                    if (link.prevEntity != null)
                    {
                        link.prevEntity.GetComponent<Components.Selectable<T>>().selected = true;
                        Console.WriteLine("UP");
                        selectable.selected = false;
                        sound.play = true;
                    }
                    })
            },
            {
                Shared.Controls.ControlContext.MenuDown,
                    new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                    {
                    Shared.Components.Selectable<T> selectable = entity.GetComponent<Shared.Components.Selectable<T>>();
                    Shared.Components.Linkable link = entity.GetComponent<Shared.Components.Linkable>();
                    Shared.Components.Audible sound = entity.GetComponent<Shared.Components.Audible>();
                    if (link.prevEntity != null)
                    {
                        link.nextEntity.GetComponent<Components.Selectable<T>>().selected = true;
                        Console.WriteLine("DOWN");
                        selectable.selected = false;
                        sound.play = true;
                    }
                    })
            },
            {
                Shared.Controls.ControlContext.Confirm,
                new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                {
                    Shared.Components.Selectable<T> selectable = entity.GetComponent<Shared.Components.Selectable<T>>();
                    Console.WriteLine("ENTER");
                    selectable.interacted = true;
                })
            }
        };

    }
}
