using Microsoft.Xna.Framework;

namespace FinLeafIsle.Collisions
{
    public class CameraBlock
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Velocity;
        public AABB BoundingBox => new AABB(Position - Size / 2f, Position + Size / 2f);

        public CameraBlock(Vector2 position, Vector2 size)
        {
            Position = position;    
            Size = size;
        }
    }
}
