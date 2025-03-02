using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moon_River
{
    internal class NPC : GameObject
    {
        // fields
        private List<string> dialogue;
        private int dialogueLine;
        
        // properties
        public List<string> Script { get => dialogue; set => dialogue = value; }
        public int CurrentLine { get => dialogueLine; }

        // constructor
        public NPC(Rectangle location, Texture2D texture, string filename)
            :base(location, texture)
        {
            this.location = location;
            this.texture = texture;

            dialogue = new List<string>();
            dialogueLine = 0;

            // file input
            StreamReader input;
            try
            {
                input = new StreamReader(filename);
                string line;
                while ((line = input.ReadLine()) != null!)
                {
                    dialogue.Add(line);
                }
            }
            catch (Exception e)
            {
                Console.Write (e.Message);
            }
        }

        // method
        public bool CanTalk(Player player, Vector2 worldPos)
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
