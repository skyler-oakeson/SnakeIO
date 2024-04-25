using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;
using Systems;
using Microsoft.Xna.Framework.Audio;
using Shared.Systems;
using Shared.Entities;
using Shared;
using System.Collections.Generic;
using System.Reflection;

namespace Scenes
{
    public class ScoreScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Selector<SceneContext> selector;
        private Audio audio;

        private SpriteFont font;


        private Shared.DataManager dataManager;
        private List<ulong>? scoreValues;
        private List<ulong>? scoresOld;

        private List<Shared.Entities.Entity> entityList = new List<Shared.Entities.Entity>();


        public ScoreScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager, Shared.DataManager dataManager, ref List<ulong> scores)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.selector = new Systems.Selector<SceneContext>();
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();
            this.dataManager = dataManager;
            this.scoreValues = scores;
        }

        override public void LoadContent(ContentManager contentManager)
        {

            int center = graphics.PreferredBackBufferWidth / 2;
            font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            SoundEffect sound = contentManager.Load<SoundEffect>("Audio/click");
            AddEntity(Shared.Entities.StaticText.Create(font, "High Scores", Color.Black, Color.Orange, new Rectangle(center - (int)font.MeasureString("High Scores").X / 2, 50 + (int)font.MeasureString("High Scores").Y / 2, 0, 0)));


            // make a copy of the highscores to check for updates 

            scoreValues.Sort(); // Ensure the highest is last and all in order 
            scoreValues.Reverse(); // Ensure the highest is first 
            scoresOld = new List<ulong>(scoreValues);
            scoresOld.Sort();
            scoresOld.Reverse();

            if (scoreValues.Count == 0)
            {

                entityList.Add(Shared.Entities.StaticText.Create(font, "No Scores", Color.Black, Color.Orange, new Rectangle(center - (int)font.MeasureString("No Scores").X / 2, 50 + (int)font.MeasureString("No Scores").Y, 0, 0)));

            }
            else
            {
                scoreValues.Sort(); // Ensure the highest is last and all in order 
                scoreValues.Reverse(); // Ensure the highest is first 



                for (int i = 0; i < scoreValues.Count; i++)
                {   // Value, entity to add 
                    string value = scoreValues[i].ToString();
                    entityList.Add(Shared.Entities.StaticText.Create(font, value, Color.Black, Color.Orange, new Rectangle(center - (int)font.MeasureString(value).X / 2, 50 + (50 * (i + 1)) + (int)font.MeasureString(value).Y, 0, 0)));
                }

            }

            foreach (var entity in entityList)
            {
                AddEntity(entity);
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

            if (scoreValues.Count > 5)
            {
                backUpPrune();
            }


            if (scoreValues != null && !scoreValues.Equals(scoresOld))
            {
                int center = graphics.PreferredBackBufferWidth / 2;
                foreach (var entity in entityList)
                {
                    RemoveEntity(entity);
                }

                entityList.Clear();

                scoreValues.Sort();
                scoreValues.Reverse();
                scoresOld = new List<ulong>(scoreValues); // Remake the new list 

                // new Rectangle(center - (int)font.MeasureString(value).X / 2, 50 + (50 * (i + 1)), 0, 0), font, value

                for (int i = 0; i < scoreValues.Count; i++)
                {   // Value, entity to add 
                    string value = $"{(i + 1)}. {scoreValues[i].ToString()}";
                    entityList.Add(Shared.Entities.StaticText.Create(font, value, Color.Black, Color.Orange, new Rectangle(center - (int)font.MeasureString(value).X / 2, 50 + (50 * (i + 1)) + (int)font.MeasureString(value).Y, 0, 0)));
                }

                foreach (var entity in entityList)
                {
                    AddEntity(entity);
                }


            }

            renderer.Update(elapsedTime);
            selector.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            audio.Update(elapsedTime);
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

        private void backUpPrune()
        {
            scoreValues.Sort();
            scoreValues.RemoveAt(0);
            scoreValues.Reverse();

        }
    }
}

