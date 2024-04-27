using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;
using Systems;
using System.Collections.Generic;

namespace Scenes
{
    public class ScoreScene : Scene
    {
        public delegate void SaveScores(float score);
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<SceneContext> selector;
        private Audio audio;
        private SpriteFont font;
        private Shared.DataManager dm;
        private List<Shared.HighScores.HighScore> scores = new List<Shared.HighScores.HighScore>();
        private List<Shared.Entities.Entity> entityList = new List<Shared.Entities.Entity>();
        public SaveScores saveScore;

        public ScoreScene(GraphicsDevice graphicsDevice, 
                          GraphicsDeviceManager graphics,
                          Shared.Controls.ControlManager controlManager,
                          Shared.DataManager dataManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.dm = dataManager;
            this.renderer = new Renderer(spriteBatch);
            this.scores = dm.Load<List<Shared.HighScores.HighScore>>(scores);
            this.saveScore = new SaveScores(
                (float score) => {
                    scores.Add(new Shared.HighScores.HighScore((int)score));
                    dm.Save<List<Shared.HighScores.HighScore>>(scores);
                });

        }

        override public void LoadContent(ContentManager contentManager)
        {
            int center = graphics.PreferredBackBufferWidth / 2;
            font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            int[] array = new int[] { 3, 1, 4, 5, 2 };
            scores.Sort(new Comparison<Shared.HighScores.HighScore>((h1, h2) => h2.score.CompareTo(h1.score)));
            AddEntity(Shared.Entities.StaticText.Create(
                        font, "HIGHSCORE", Color.Black,
                        Color.White, new Rectangle((int)((screenWidth/2)-font.MeasureString("HIGHSCORE").X/2), 100, 0, 0)));

            int limit = scores.Count <= 5 ? scores.Count : 5;
            for (int i = 0; i < limit; i++)
            {
                Shared.Entities.Entity hudElement = Shared.Entities.StaticText.Create(font, $"{i}. {scores[i].score}", Color.Black, Color.White, 
                        new Rectangle(((screenWidth/2)-(int)font.MeasureString($"{i}. {scores[i].score}").X/2), (int)(50*i) + 200, (int)font.MeasureString($"{i}. {scores[i].score}").X, (int)font.MeasureString("").Y));
                AddEntity(hudElement);
            }
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return SceneContext.MainMenu;
            }
            return SceneContext.Scores;
        }

        override public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime);
        }

        override public void Update(TimeSpan elapsedTime)
        {
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            renderer.Remove(entity.id);
        }
    }
}

