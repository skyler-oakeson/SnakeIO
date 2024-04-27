using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Systems;
using Shared;

namespace Scenes
{
    public class GameOverScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<SceneContext> selector;
        private Audio audio;
        private Shared.Systems.Linker linker;
        private SpriteFont font;
        private Shared.Entities.Entity playerKills;
        private Shared.Entities.Entity playerPlace;
        private Shared.Entities.Entity playerScore;
        private DataManager dm = new DataManager();
        private List<ulong> highscores;

        public GameOverScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.renderer = new Renderer(spriteBatch);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<SceneContext>();
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();
            this.linker = new Shared.Systems.Linker();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            this.font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Shared.Entities.StaticText.Create(
                        font, "GAMEOVER", Color.Black, Color.Red,
                        new Rectangle((int)((screenWidth / 2) - font.MeasureString("GAMEOVER").X / 2), 100, 0, 0)));
            this.playerScore = Shared.Entities.StaticText.Create(
                    font, "", Color.Black, Color.White, 
                    new Rectangle((int)((screenWidth / 2) - font.MeasureString("Score : ").X / 2), 200, 0, 0));

            AddEntity(playerScore);
            AddEntity(Shared.Entities.MenuItem<SceneContext>.Create(
                        font, SceneContext.MainMenu, "GameOver",
                        true, sound, Shared.Components.LinkPosition.Body,
                        controlManager, new Rectangle(30, screenHeight - 100, 0, 0)));
        }

        public void UpdatePlayerStats(string score)
        {
            playerScore.GetComponent<Shared.Components.Readable>().text = $"Score : {score}";
            playerScore.GetComponent<Shared.Components.Readable>().rectangle = new Rectangle((int)((screenWidth / 2) - font.MeasureString("Score : ").X / 2), screenHeight-100, 0, 0);
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            selector.Update(gameTime.ElapsedGameTime);

            // Return selected scene
            if (selector.selectedVal != default(SceneContext))
            {
                dm.Save(highscores);
                return selector.selectedVal;
            }

            return SceneContext.Game;
        }

        override public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
        }

        override public void Update(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
            selector.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            audio.Update(elapsedTime);
            linker.Update(elapsedTime);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            selector.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
            linker.Add(entity);
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            renderer.Remove(entity.id);
            selector.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
            linker.Remove(entity.id);
        }
    }
}

