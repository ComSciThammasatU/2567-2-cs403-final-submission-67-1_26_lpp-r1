using Microsoft.Xna.Framework;
using System.Collections.Generic;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components.ItemComponent;
using MonoGame.Extended.ECS;

namespace FinLeafIsle.Components.Inventory
{
    public class InventorySlot
    {
        public bool isPressed, isHovered;
        public Entity _item { get; set; } = null;

        public Vector2 Position;
        public AABB BoundingBox => new AABB(Position - Size / 2f, Position + Size / 2f);
        public Vector2 Size;

    }
}
