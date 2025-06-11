using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FinLeafIsle.Components.ItemComponent;

namespace FinLeafIsle.Collisions
{
    
    public class WaterArea
    {
        public WaterArea(Vector2 position, Vector2 size, int depth)
        {
            Position = position;
            Size = size;
            Depth = depth;
        }

        public Vector2 Position;
        public AABB BoundingBox => new AABB(Position - Size / 2f, Position + Size / 2f);
        public Vector2 Size;
        public int Depth { get; private set; }
        public List<Fish> _fishData = new List<Fish>();
        public List<Item> _fishItem = new List<Item>();

        public void AddFish(Fish fish, Item item)
        {
            _fishData.Add(fish);
            _fishItem.Add(item);
        }

    }
}
