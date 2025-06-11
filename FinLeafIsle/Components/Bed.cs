using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FinLeafIsle.Collisions;

namespace FinLeafIsle.Components
{

    public class Bed
    {
        public Vector2 Position;
        public Vector2 Size;
        public AABB BoundingBox => new AABB(Position - Size / 2f, Position + Size / 2f);

    }
}
