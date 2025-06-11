using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLeafIsle
{
    public static class DebugOverlay
    {
        private static List<string> _lines = new List<string>();

        public static void AddLine(string line)
        {
            _lines.Add(line);
        }

        public static void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                spriteBatch.DrawString(font, _lines[i], new Vector2(10, 10 + i * 20), Color.White);
            }
            _lines.Clear();
        }
    }
}
