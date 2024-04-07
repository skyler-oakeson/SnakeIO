using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Systems
{
    /// <summary>
    /// </summary>
    public class Selector<T> : System
    {
        public T selectedVal = default(T);

        public Selector()
            : base(
                   typeof(Components.Selectable<T>)
                   )
        {
        }


        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                Components.Selectable<T> sel = entity.GetComponent<Components.Selectable<T>>();
                if (sel.prevState != sel.selected)
                {
                    Select(entity);
                }
                if (sel.interacted)
                {
                    sel.interacted = false;
                    selectedVal = sel.value;
                    if (sel.selectableDelegate != null)
                    {
                        sel.interacted = !sel.selectableDelegate();
                    }
                }
            }
        }


        /// <summary>
        /// </summary>
        public void Select(Entities.Entity entity)
        {
            Components.Selectable<T> sel = entity.GetComponent<Components.Selectable<T>>();

            if (entity.ContainsComponent<Components.Renderable<SpriteFont>>())
            {
                Components.Renderable<SpriteFont> renderable = entity.GetComponent<Components.Renderable<SpriteFont>>();
                Color temp = renderable.stroke;
                renderable.stroke = renderable.color;
                renderable.color = temp;
            }

            if (entity.ContainsComponent<Components.KeyboardControllable>())
            {
                entity.GetComponent<Components.KeyboardControllable>().enable = sel.selected;
            }


            sel.prevState = !sel.prevState;
        }
    }
}
