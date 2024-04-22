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
                string text,
                bool selected,
                int x,
                int y)
        {
            Entity textInput = new Entity();
            textInput.Add(new Shared.Components.Appearance());
            textInput.Add(new Shared.Components.Readable(text, Color.Orange, Color.Black, new Rectangle(x, y-(int)(font.MeasureString("1").X), 0, 0), font: font));
            textInput.Add(new Shared.Components.Audible(sound));
            textInput.Add(new Shared.Components.Positionable(new Vector2(x, y), 0f));
            textInput.Add(new Shared.Components.Selectable<string>(selected, text, selectableDelegate: TextBoxEdit, interactableDelegate: TextBoxConfirm));
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
                if (((int)consumed >= 0x41 && (int)consumed <= 0x5A) && readable.text.Length <= 12)
                {
                    readable.text += consumed;
                    audible.play = true;
                    Vector2 textSize = readable.font.MeasureString(consumed.ToString());
                    readable.rectangle = new Rectangle(
                            readable.rectangle.X - ((int)textSize.X/2),
                            readable.rectangle.Y,
                            0, 0);
                }
                else if (consumed == Keys.Back)
                {
                    if (readable.text.Length > 0)
                    {
                        char delLetter = readable.text[readable.text.Length-1];
                        readable.text = readable.text.Remove(readable.text.Length-1);
                        audible.play = true;
                        Vector2 textSize = readable.font.MeasureString(delLetter.ToString());
                        readable.rectangle = new Rectangle(
                                readable.rectangle.X + ((int)textSize.X/2),
                                readable.rectangle.Y,
                                0, 0);
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
