using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    /// <summary>
    /// </summary>
    public class Selector<T> : Shared.Systems.System
    {
        public T selectedVal = default(T);

        public Selector()
            : base(
                   typeof(Shared.Components.Selectable<T>)
                   )
        {
        }


        public override void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in entities.Values)
            {
                Shared.Components.Selectable<T> sel = entity.GetComponent<Shared.Components.Selectable<T>>();
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
        public void Select(Shared.Entities.Entity entity)
        {
            Shared.Components.Selectable<T> sel = entity.GetComponent<Shared.Components.Selectable<T>>();

            if (entity.ContainsComponent<Shared.Components.Readable>())
            {
                Shared.Components.Readable readable = entity.GetComponent<Shared.Components.Readable>();
                Color temp = readable.stroke;
                readable.stroke = readable.color;
                readable.color = temp;
            }

            if (entity.ContainsComponent<Shared.Components.KeyboardControllable>())
            {
                entity.GetComponent<Shared.Components.KeyboardControllable>().enable = sel.selected;
            }


            sel.prevState = !sel.prevState;
        }
    }
}