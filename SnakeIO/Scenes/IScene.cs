using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Scenes
{
    public interface IScene 
    {
        protected void Initialize(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager);
        public void LoadContent(ContentManager contentManager);
        public SceneContext ProcessInput(GameTime gameTime);
        public void Update(TimeSpan elapsedTime);
        public void Render(TimeSpan elapsedTime);
    }
}
