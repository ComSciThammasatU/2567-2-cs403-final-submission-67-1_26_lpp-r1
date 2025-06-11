using Microsoft.Xna.Framework;
using FinLeafIsle.Collisions;
using Microsoft.Xna.Framework.Graphics;

namespace FinLeafIsle.Components.UI
{
    public class Button
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Offset;
        public AABB BoundingBox => new AABB(Position - Size / 2f, Position + Size / 2f);
        public Button() { }
        public Texture2D Texture;
    }
}
