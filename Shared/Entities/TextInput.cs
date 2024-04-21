using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Shared.Entities
{
    public class TextInput
    {
        public static Entity Create(
                SpriteFont font,
                Texture2D background,
                SoundEffect sound,
                string value,
                bool selected,
                Rectangle rectangle)
        {
            Entity textInput = new Entity();
            textInput.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), Color.White, Color.White, rectangle));
            textInput.Add(new Shared.Components.Renderable(background, "Fonts/Micro5", Color.White, Color.White, rectangle));
            textInput.Add(new Shared.Components.Readable(font, "Fonts/Micro5", value.ToString(), Color.Orange, Color.Black, rectangle));
            textInput.Add(new Shared.Components.Audible(sound));
            textInput.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));
            textInput.Add(new Shared.Components.Selectable<string>(selected, value, selectableDelegate: TextBoxEdit, interactableDelegate: TextBoxConfirm));
            textInput.Add(new Shared.Components.KeyboardControllable(selected, typeof(Shared.Entities.TextInput), TextInputControls));

            return textInput;
        }

        public static Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate> TextInputControls = 
            new Dictionary<Shared.Controls.ControlContext, Shared.Controls.ControlDelegate>
        {
            {
                Shared.Controls.ControlContext.Confirm,
                new Shared.Controls.ControlDelegate((Entities.Entity entity, TimeSpan elapsedTime) =>
                {
                    Shared.Components.Selectable<string> selectable = entity.GetComponent<Shared.Components.Selectable<string>>();
                    selectable.interacted = true;
                })
            }
        };

        private static Shared.Components.SelectableDelegate TextBoxEdit = new Components.SelectableDelegate((Shared.Entities.Entity entity) => 
        {
            Shared.Components.Readable readable = entity.GetComponent<Shared.Components.Readable>();
            Shared.Components.Audible audible = entity.GetComponent<Shared.Components.Audible>();
            Shared.Components.KeyboardControllable keyboard = entity.GetComponent<Shared.Components.KeyboardControllable>();
            Keys? consumed = keyboard.ConsumeKeyPress();
            if (consumed != null)
            {
                if (((int)consumed >= 0x41 && (int)consumed <= 0x5A) && readable.text.Length <= 6)
                {
                    readable.text += consumed;
                    audible.play = true;
                }
                else if (consumed == Keys.Back)
                {
                    if (readable.text.Length > 0)
                    {
                        readable.text = readable.text.Remove(readable.text.Length-1);
                        audible.play = true;
                    }
                }
            }
            return true;
        });

        private static Shared.Components.InteractableDelegate TextBoxConfirm = new Components.InteractableDelegate((Shared.Entities.Entity entity) =>
        {
            Shared.Components.Selectable<string> selectable = entity.GetComponent<Shared.Components.Selectable<string>>();
            Shared.Components.Readable readable = entity.GetComponent<Shared.Components.Readable>();
            selectable.value = readable.text;
            return true;
        });
    }
}
