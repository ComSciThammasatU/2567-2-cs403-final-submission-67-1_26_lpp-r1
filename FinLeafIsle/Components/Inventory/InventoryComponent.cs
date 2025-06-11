using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using System.Collections.Generic;
using FinLeafIsle.Components.ItemComponent;

namespace FinLeafIsle.Components.Inventory
{
    public class InventoryComponent
    {
        public List<InventorySlot> _inventorySlot = new List<InventorySlot>();
        public int _count;

        public InventoryComponent(int Count)
        {
            _count = Count;
            

            for (int i = 0; i < _count; i++)
            {
                Vector2 tempVec = new Vector2(170 + 17 * (int)(i%9), 120 + 17 * (int)(i/9));
                Vector2 size = new Vector2(16, 16);
                _inventorySlot.Add(new InventorySlot {Size = size, isHovered = false, isPressed = false, Position = tempVec});
            }
        }

        public void AddItem(Entity item)
        {
            for (int i = 0;  i < _inventorySlot.Count; i++)
            {
                if (_inventorySlot[i]._item == null)
                {
                    _inventorySlot[i]._item = item;
                    break;
                }  
            }
        }
        
    }
}
