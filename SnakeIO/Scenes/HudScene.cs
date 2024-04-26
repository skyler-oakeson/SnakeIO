using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Systems;

namespace Scenes
{
    public class HudScene : Scene
    {
        private Renderer renderer;
        private SpriteFont font;
        private List<Shared.Entities.Entity> scores;
        private Shared.Entities.Entity playerStats;

        public HudScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);

            this.renderer = new Renderer(spriteBatch);
            this.scores = new List<Shared.Entities.Entity>();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            this.font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            for (int i = 0; i < 5; i++)
            {
                float scale = .7f;
                Shared.Entities.Entity hudElement = Shared.Entities.StaticText.Create(font, "", Color.Black, Color.White, 
                        new Rectangle(30, (int)(50*i*scale), (int)font.MeasureString("").X, (int)font.MeasureString("").Y), scale);
                this.scores.Add(hudElement);
                AddEntity(hudElement);
            }
            this.playerStats = Shared.Entities.StaticText.Create(font, "", Color.Black, Color.White, 
                    new Rectangle(screenWidth-50, 10, (int)font.MeasureString("").X, (int)font.MeasureString("").Y));
            AddEntity(playerStats);
        }

        public void UpdateScores((string, float)[] updatedScores)
        {
            int limit = updatedScores.Length <= 5 ? updatedScores.Length : 5;
            for (int i = 0; i < limit; i++)
            {
                Shared.Entities.Entity curr = this.scores[i];
                (string, float) entry = updatedScores[i];
                curr.GetComponent<Shared.Components.Readable>().text = $"{entry.Item1} : {entry.Item2}";
            }
        }

        public void UpdatePlayerStats(string score)
        {
            Rectangle pos = playerStats.GetComponent<Shared.Components.Readable>().rectangle;
            Vector2 scoreSize = font.MeasureString(score);
            playerStats.GetComponent<Shared.Components.Readable>().text = score;
            playerStats.GetComponent<Shared.Components.Readable>().rectangle = new Rectangle((int)(screenWidth-50-scoreSize.X), pos.Y, (int)scoreSize.X, (int)scoreSize.Y);
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            return SceneContext.Game;
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

