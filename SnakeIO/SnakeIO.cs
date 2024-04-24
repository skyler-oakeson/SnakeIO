using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Scenes;

namespace SnakeIO
{
    public class SnakeIO : Game
    {
        public bool sceneSwapped = false;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Shared.DataManager dataManager;
        private Shared.Controls.ControlManager controlManager;
        private Dictionary<SceneContext, Scene> scenes = new Dictionary<SceneContext, Scene>();
        private SceneContext nextScene;
        private SceneContext currSceneContext;
        private Scene currScene;
        private List<ulong> highScores; 

        public SnakeIO()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            dataManager = new Shared.DataManager();
            this.controlManager = new Shared.Controls.ControlManager(dataManager);
        }

        protected override void Initialize()
        {
            // graphics.PreferredBackBufferWidth = 1920;
            // graphics.PreferredBackBufferHeight = 1080;
            // graphics.ApplyChanges();

            highScores = dataManager.Load<List<ulong>>(highScores);
            
            if (highScores == null)
            {
                highScores = new List<ulong>();
            }

            scenes.Add(SceneContext.MainMenu, new MainMenuScene(graphics.GraphicsDevice, graphics, controlManager));
            scenes.Add(SceneContext.Options, new OptionScene(graphics.GraphicsDevice, graphics, controlManager));
            scenes.Add(SceneContext.Scores, new ScoreScene(graphics.GraphicsDevice, graphics, controlManager, dataManager, ref highScores));
            scenes.Add(SceneContext.Game, new GameScene(graphics.GraphicsDevice, graphics, controlManager, ref highScores));


            foreach (Scene scene in scenes.Values)
            {
                scene.Initialize(graphics.GraphicsDevice, graphics, controlManager);
            }

            currSceneContext = SceneContext.MainMenu;
            currScene = scenes[currSceneContext];
            nextScene = currSceneContext;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MessageQueueClient.instance.initialize("localhost", 3000);
            foreach (Scene scene in scenes.Values)
            {
                scene.LoadContent(this.Content);
            }
           

        }

        protected override void Update(GameTime gameTime)
        {
            HandleSceneChange(gameTime);
            currScene.Update(gameTime.ElapsedGameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            currScene.Render(gameTime.ElapsedGameTime);
            base.Draw(gameTime);
        }

        private void HandleSceneChange(GameTime gameTime)
        {
            nextScene = currScene.ProcessInput(gameTime);
            if (nextScene == SceneContext.Exit)
            {
                
                MessageQueueClient.instance.sendMessage(new Shared.Messages.Disconnect());
                MessageQueueClient.instance.shutdown();

                dataManager.Save<List<ulong>>(highScores); // save the scores.
                lockout();
                Exit();
            }
            else if (currSceneContext != nextScene)
            {
                sceneSwapped = true;
                currScene = scenes[nextScene];
                currSceneContext = nextScene;
                currScene.SwapScene();
            }
        }


        private void lockout()
        {
            while (dataManager.saving)
            {
                //wait
            }
        }
    }
}
