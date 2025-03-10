﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
// using System.Numerics;

namespace Moon_River
{
    internal class GameObject
    {
        // fields
        protected Rectangle location;
        protected Texture2D texture;

        // properties
        public int X { get => location.X; set => location.X = value; }
        public int Y { get => location.Y; set => location.Y = value; }
        public Rectangle Location { get => location; set => location = value; }

        // constructor
        public GameObject(Rectangle location, Texture2D texture)
        {
            this.location = location;
            this.texture = texture;
        }

        // methods
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                location,
                null,
                Microsoft.Xna.Framework.Color.White,
                0,
                new Vector2(),
                SpriteEffects.None,
                0);
        }
    }
}
