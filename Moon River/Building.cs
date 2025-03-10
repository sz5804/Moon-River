﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
// using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_River
{
    internal class Building : GameObject
    {
        // field 
        private bool inBuilding;

        // property
        public bool Occupied { get => inBuilding; set => inBuilding = value; }

        // constructor
        public Building(Rectangle location, Texture2D texture)
            : base(location, texture)
        {
            this.location = location;
            this.texture = texture;
            inBuilding = false;
        }

        // methods
        public bool CanEnterBuilding(Player player, Vector2 worldPos)
        {
            Rectangle currentPos = this.Reposition(worldPos);
            if (player.X + player.Location.Width < currentPos.X)
            {
                return false;
            }
            if (player.X > currentPos.X + location.Width)
            {
                return false;
            }
            if (player.Y + player.Location.Height < currentPos.Y)
            {
                return false;
            }
            if (player.Y > currentPos.Y + location.Height)
            {
                return false;
            }
            return true;
        }

        public Rectangle Reposition(Vector2 worldPos)
        {
            int x = this.X - (int)worldPos.X;
            int y = this.Y - (int)worldPos.Y;
            return new Rectangle(x, y, location.Width, location.Height);
        }
    }
}
