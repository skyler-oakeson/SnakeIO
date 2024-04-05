using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scenes
{
    public interface IScene 
    {
        protected void Initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager);
        public void LoadContent(ContentManager contentManager);
        public SceneContext ProcessInput(GameTime gameTime);
        public void Update(GameTime gameTime);
        public void Render(GameTime gameTime);
    }
}
