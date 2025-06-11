using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLeafIsle.Datas
{
    public class SavedMapData
    {
        public string MapName { get; set; }
        public List<SavedEntityData> Entities { get; set; } = new();
    }
    public class SavedEntityData
    {
        public string ContainSlot { get; set; }
        public string EntityType { get; set; }
        public Box Box { get; set; }
        public Fish Fish { get; set; }
        public FishingRod FishingRod { get; set; }
        public Player Player { get; set; }
        public Bed Bed { get; set; }

        
        public Transform2 Transform { get; set; }
        public Body Body { get; set; }
        public Item Item { get; set; }

        public List<SavedEntityData> ContainItem { get; set; } = new();
    }

    public class SavedGameData
    {
        public int Day { get; set; }
        public MapLocation BedLocation { get; set; }
    }
}