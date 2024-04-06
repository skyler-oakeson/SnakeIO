using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Controls;
using Scenes;

namespace SnakeIO 
{
    public class SnakeIO : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private DataManager dataManager;
        private ControlManager controlManager;
        private Dictionary<SceneContext, Scene> scenes = new Dictionary<SceneContext, Scene>();
        private SceneContext nextScene;
        private SceneContext currSceneContext;
        private Scene currScene;

        public SnakeIO()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            dataManager = new DataManager();
            controlManager = new ControlManager(dataManager);
        }

        protected override void Initialize()
        {
            scenes.Add(SceneContext.Game, new GameScene(graphics.GraphicsDevice, graphics, controlManager));
            scenes.Add(SceneContext.MainMenu, new MainMenuScene(graphics.GraphicsDevice, graphics, controlManager));

            foreach (Scene scene in scenes.Values)
            {
                scene.Initialize(graphics.GraphicsDevice, graphics, controlManager);
            }

            currSceneContext = SceneContext.Game;
            currScene = scenes[currSceneContext];
            nextScene = currSceneContext;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (Scene scene in scenes.Values)
            {
                scene.LoadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            // {
            //     Exit();
            // }

            if (currSceneContext != nextScene)
            {
                currScene = scenes[nextScene];
                currSceneContext = nextScene;
            }

            nextScene = currScene.ProcessInput(gameTime);
            currScene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            currScene.Render(gameTime);
            base.Draw(gameTime);
        }
    }
}
