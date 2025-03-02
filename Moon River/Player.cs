using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Common;
// using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_River
{
    internal class Player : GameObject
    {
        // fields
        private Texture2D[] anim;
        private int currentFrame;
        private int frameCount;
        private bool walk;

        // property
        public bool Walking { get => walk; set => walk = value; }

        // constructor
        public Player(Rectangle location, Texture2D[] anim)
            : base(location, anim[0])
        {
            this.location = location;
            this.anim = anim;
            currentFrame = 0;
            walk = false;
        }

        // methods
        public override void Update(GameTime gameTime)
        {
            if (walk)
            {
                frameCount++;
                if (frameCount%7 == 1)
                {
                    currentFrame++;
                }
                currentFrame %= anim.Length;
            }
            else
            {
                currentFrame = 0;
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                anim[currentFrame],
                location,
                null,
                Color.White,
                0f,
                new Vector2(),
                SpriteEffects.None,
                0);
            base.Draw(spriteBatch);
        }
    }
}
