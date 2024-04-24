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
        private Shared.HighScores scoreValues;
        private List<ulong> scoresOld;

        private List< Shared.Entities.Entity> entityList = new List< Shared.Entities.Entity >();


        public ScoreScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager, Shared.DataManager dataManager, HighScores scores)
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
            AddEntity(Shared.Entities.Score.create(new Rectangle(center - (int)font.MeasureString("Scores").X/2, 50 + (int)font.MeasureString("Scores").Y / 2, 0, 0), font, "Scores"));

            //scoreValues = dataManager.Load<Shared.HighScores>(scoreValues);

            if (scoreValues == null || scoreValues.highScores.Count == 0)
            {
                scoreValues = new Shared.HighScores(new List<ulong>());
                entityList.Add( Score.create(new Rectangle(center - (int)font.MeasureString("No Scores Yet").X / 2, 100 + (int)font.MeasureString("No Scores Yet").Y / 2, 0, 0), font, "No Scores Yet"));
            }
            else
            {
                scoreValues.highScores.Sort(); // Ensure the highest is last and all in order 
                scoreValues.highScores.Reverse(); // Ensure the highest is first 

                scoresOld = new List<ulong>(scoreValues.highScores); // make a copy of the highscores to check for updates 

                foreach (var value in scoreValues.highScores)
                {   // Value, entity to add 
                    entityList.Add(Score.create(new Rectangle(center - (int)font.MeasureString(value.ToString()).X / 2, 50, 0, 0), font, value.ToString()));
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

            if(scoreValues.highScores != scoresOld)
            {
                int center = graphics.PreferredBackBufferWidth / 2;
                foreach (var entity in entityList)
                {
                    RemoveEntity(entity);
                }

                scoreValues.highScores.Sort();
                scoreValues.highScores.Reverse();
                scoresOld = new List<ulong>(scoreValues.highScores); // Remake the new list 

                foreach (var value in scoreValues.highScores)
                {
                    entityList.Add(Score.create(
                        new Rectangle(center - (int)font.MeasureString(value.ToString()).X / 2, 50 + (50 * (scoreValues.highScores.IndexOf(value) + 1)) + (int)font.MeasureString(value.ToString()).Y / 2, 0, 0), 
                        font, value.ToString()));
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


    }
}

