using FinLeafIsle.Components.ItemComponent;
using System.Collections.Generic;

public class FishJsonData
{
    public int Id;
    public string Name;
    public string Description;
    public int Difficult;
    public int Depth;
    public string Diet;
    public float SpawnChance;
    public int Start;
    public int End;
    public List<string> Location;
    public FishBehaviorDefinition Behavior;
}
