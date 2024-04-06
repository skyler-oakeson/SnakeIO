using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Systems
{
    /// <summary>
    /// </summary>
    public class Selector : System
    {
        private Scenes.SceneContext sc;
        public Selector(Scenes.SceneContext sc)
            : base(
                   typeof(Components.Selectable)
                   )
        {
            this.sc = sc;
        }


        public override void Update(GameTime gameTime)
        {
            foreach (var entity in entities.Values)
            {
                Select(entity, gameTime);
            }
        }


        /// <summary>
        /// </summary>
        public Scenes.SceneContext Select(Entities.Entity entity, GameTime gameTime)
        {
            Components.Selectable sel = entity.GetComponent<Components.Selectable>();
            if (sel.selected)
            {
                sel.selected = false;
                return sel.selectionDelegate();
            }
            return sc;
        }
    }
}
