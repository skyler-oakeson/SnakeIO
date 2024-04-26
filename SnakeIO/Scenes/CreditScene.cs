using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;
using Systems;
using System.Collections.Generic;
using Shared.Entities;
using Shared.Components;
using System.Diagnostics;


namespace Scenes
{
    public class CreditScene : Scene
    {
        private Renderer renderer;
        private KeyboardInput keyboardInput;
        private Audio audio;
        private SpriteFont font;

        private Vector2 center;

        private double interval = 3;
        private double timeSince = 0;
        private List<Shared.Entities.Entity> entities = new List<Shared.Entities.Entity>();
        private Texture2D fader;
        private float amountFade = 0.0f;
        private bool shouldFade = false;
        private bool playing = true;
        private int stage = 0;
        private String[] CREDITSEQUENCE = {"Credits" ,
                                           "Programer : Skyler Oakeson",
                                           "Programer : Zane Hirning",
                                           "Programer : Robert Gordon",
                                           "Art by : Dean Oakeson ",
                                           "Main song : Snake Song",
                                           "Music by : ",
                                           "Preston Tengren ",
                                           "And Jaden Storrer",
                                           "Made For",
                                           " C S 5410 ",
                                           " Thanks for Playing "
        };
        public CreditScene(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, Shared.Controls.ControlManager controlManager)
        {
            this.Initialize(graphicsDevice, graphics, controlManager);
            this.controlManager = controlManager;
            this.keyboardInput = new Systems.KeyboardInput(controlManager);
            this.renderer = new Renderer(spriteBatch);
            this.audio = new Audio();
        }

        override public void LoadContent(ContentManager contentManager)
        {
            center = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            font = contentManager.Load<SpriteFont>("Fonts/Micro5-50");
            fader = contentManager.Load<Texture2D>("Images/square");

            foreach (var credit in CREDITSEQUENCE)
            {
                entities.Add(Shared.Entities.StaticText.Create(font, credit, Color.Black, Color.Orange, new Rectangle((int)center.X - (int)font.MeasureString(credit).X / 2, (int)center.Y - (int)font.MeasureString(credit).Y / 2, 0, 0)));
            }
        }

        override public SceneContext ProcessInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                reset();
                return SceneContext.MainMenu;
            }

            return SceneContext.Credits;
        }


        override public void Render(TimeSpan elapsedTime)
        {
            renderer.Update(elapsedTime); // Make whatever Renderer wants to do first
            drawFade(elapsedTime);
        }

        override public void Update(TimeSpan elapsedTime)
        {
            animation(elapsedTime);
            renderer.Update(elapsedTime);
            keyboardInput.Update(elapsedTime);
            audio.Update(elapsedTime);
        }

        private void AddEntity(Shared.Entities.Entity entity)
        {
            renderer.Add(entity);
            keyboardInput.Add(entity);
            audio.Add(entity);
        }

        private void RemoveEntity(Shared.Entities.Entity entity)
        {
            renderer.Remove(entity.id);
            keyboardInput.Remove(entity.id);
            audio.Remove(entity.id);
        }

        private void drawFade(TimeSpan elapsedTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(fader, new Rectangle(0,0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Black * amountFade);
            spriteBatch.End();
        }

        private void animation(TimeSpan elapsedTime)
        {
            if (stage == CREDITSEQUENCE.Length - 1 && playing)
            {
                playing = false;
            }

            if (playing)
            {
                RemoveEntity(entities[stage]);
                if (timeSince < interval && !shouldFade)
                {
                    timeSince += elapsedTime.TotalSeconds;
                }
                if (timeSince > interval && !shouldFade)
                {
                    shouldFade = true;
                }
                if (timeSince > 0f && shouldFade)
                {
                    timeSince -= elapsedTime.TotalSeconds;
                }
                if (timeSince <= 0f && shouldFade)
                {
                    shouldFade = false;
                    stage += 1;
                }


                if (shouldFade)
                {
                    amountFade += .01f;
                    if (amountFade > 1.0f) { amountFade = 1.0f; }

                }
                if (!shouldFade)
                {
                    amountFade -= .01f;
                    if (amountFade < 0.0f) { amountFade = 0.0f; }
                }
                AddEntity(entities[stage]);
            }
            else
            {
                if (amountFade > 0) { amountFade -= 0.01f; }
                else { amountFade = 0.0f; }
            }
        }

        public void reset()
        {
            amountFade = 0.0f;
            shouldFade = false;
            playing = true;
            RemoveEntity(entities[stage]);
            stage = 0;
            timeSince = 0f;
        }
    }
}

