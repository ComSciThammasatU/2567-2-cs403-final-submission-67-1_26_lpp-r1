using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FinLeafIsle.Components.ItemComponent;


namespace FinLeafIsle.Collisions
{

    public class GateArea
    {
        public Vector2 Position;
        public Vector2 Size;
        public AABB BoundingBox => new AABB(Position - Size / 2f, Position + Size / 2f);
        public string Destination = "";
        public Vector2 Target;
        public GateArea(Vector2 position, Vector2 size, string location, Vector2 target)
        {
            Position = position;
            Size = size;
            Destination = location;
            Target = target;
        }

    }
}
