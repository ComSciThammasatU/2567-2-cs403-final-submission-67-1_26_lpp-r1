using Microsoft.Xna.Framework;
using System.Collections.Generic;



namespace FinLeafIsle.Components.ItemComponent
{

    public enum ItemType
    {
        Fish,
        Rod,
        Material,
        Tool,
        Consumable
    }
    public class Item
    {
        public int Id;
        public string Name;
        public string Description;
        public int StackSize { get; set; } = 1; // Default to non-stackable
        
        public Item(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }


    public class FishingRod
    {
        public int MaxCastingDistance;
        public float Strength;
        public float MaxChargeTime = 1.4f;

        public FishingRod(int maxCastingDistance, float strength)
        {
            MaxCastingDistance = maxCastingDistance;
            Strength = strength;
        }
    }

    
    public enum Diet
    {
        Harbivore, Carnivore, Omnivore
    }
    public class Fish
    {
        //public List<String> Seasons;
        //public String Weather;
        public int Start;
        public int End;
        public int Difficult;
        public float SpawnChance;
        public Diet Diet;
        public int Depth;
        public List<string> Location;

        public FishBehaviorDefinition Behavior;
        public List<Vector2> MovementPattern;
        public int CurrentPattern = 0;
        public float MoveTimer = 0f;
        public float CurrentMoveTime = 0.5f;

        public bool IsTired = false;
        public float TiredTimer = 0f;


        public Fish() { }

    }
    public class Box
    {
        public Box() { }
    }
}


