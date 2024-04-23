using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Shared.Entities
{
    public class Score
    {

        public static Entity create( Rectangle rectangle, SpriteFont font, String value)
        {
            Entity score = new Entity();

           
            score.Add(new Shared.Components.Appearance("Fonts/Micro5", typeof(SpriteFont), Color.Orange, Color.Black, rectangle));
            score.Add(new Shared.Components.Readable(font, "Fonts/Micro5", value.ToString(), Color.Orange, Color.Black, rectangle));
            score.Add(new Shared.Components.Positionable(new Vector2(rectangle.X, rectangle.Y), 0f));

            return score;
        }


    }
}
