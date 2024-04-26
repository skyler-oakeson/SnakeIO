using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Shared;

namespace Scenes
{
    public class GameScene : Scene
    {
        private SnakeIO.GameModel gameModel;
        private Systems.Renderer renderer;
        private Systems.KeyboardInput keyboardInput;
        private Systems.Selector<string> selector;
        private Systems.Audio audio;
        private Shared.Entities.Entity textInput;
        private Shared.Entities.Entity outline;
        private Shared.Entities.Entity textBox;
        private ContentManager contentManager;
        private GameSceneState state = GameSceneState.Input;
        private DataManager dm;

        private List<ulong> highScores;
       

        public GameScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager, DataManager dm, ref List<ulong> highscores)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<string>();
            this.renderer = new Systems.Renderer(spriteBatch);
            this.audio = new Systems.Audio();
            this.dm = dm;
            this.highScores = highscores;
            
        }

        override public void LoadContent(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            SpriteFont font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            Texture2D background = contentManager.Load<Texture2D>("Images/text-input-bkg");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            string title = "ENTER YOUR NAME";
            this.textBox = Shared.Entities.StaticText.Create(font, title, Color.Black, Color.White, new Rectangle((int)((screenWidth/2)-(font.MeasureString(title).X/2)), 50, 0, 0));
            this.textInput = Shared.Entities.TextInput.Create(font, background, sound, "", true, screenWidth/2, (screenHeight/2)-15);
            this.outline = Shared.Entities.StaticImage.Create(background, "Images/text-input-bkg", screenWidth/2, screenHeight/2);
            AddEntity(outline);
            AddEntity(textInput);
            AddEntity(textBox);
            if(highScores == null ) { highScores = new List<ulong>(); }

        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameModel.handleFinalScore();
                dm.Save(highScores);
                while(dm.saving) { }
                return SceneContext.MainMenu;
            }

            selector.Update(gameTime.ElapsedGameTime);

            if (selector.hasSelected && selector.selectedVal != "")
            {
                state = GameSceneState.Game;
                StartGame(selector.ConsumeSelection());
            }

            if (gameModel != null)
            {
                return gameModel.ProcessInput(gameTime);
            }

            return SceneContext.Game;
        }

        override public void Render(TimeSpan elapsedTime)
        {
            if (state == GameSceneState.Game)
            {
                gameModel.Render(elapsedTime);
            }
            else if (state == GameSceneState.Input)
            {
                renderer.Update(elapsedTime);
            }
        }

        override public void Update(TimeSpan elapsedTime)
        {
            if (state == GameSceneState.Game)
            {
                gameModel.Update(elapsedTime);
            }
            else if (state == GameSceneState.Input)
            {
                selector.Update(elapsedTime);
                audio.Update(elapsedTime);
                keyboardInput.Update(elapsedTime);
            }
        }

        public void StartGame(string name)
        {
            this.gameModel = new SnakeIO.GameModel(screenHeight, screenWidth, name);
            gameModel.Initialize(controlManager, spriteBatch, contentManager, graphics, ref highScores);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            selector.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            renderer.Remove(entity.id);
            selector.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
        }

        private enum GameSceneState
        {
            Input,
            Game
        }



    }
}

