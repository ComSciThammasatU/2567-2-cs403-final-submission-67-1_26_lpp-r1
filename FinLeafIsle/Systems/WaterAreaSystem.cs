using Autofac;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components.ItemComponent;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace FinLeafIsle.Systems
{
    public class WaterAreaSystem : EntitySystem
    {
        private GameState _gameState;
        private readonly GameWorld _world;
        private ComponentMapper<WaterArea> _waterAreaMapper;
        private Random _random;
        private MapLocation _currentMap;

        public WaterAreaSystem(IContainer container)
            : base(Aspect.All(typeof(WaterArea)))
        {
            _world = container.Resolve<GameWorld>();
            _gameState = container.Resolve<GameState>();
            _random = container.Resolve<Random>();
            _currentMap = container.ResolveNamed<MapLocation>("CurrentLocation");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _waterAreaMapper = mapperService.GetMapper<WaterArea>();
        }
        protected override void OnEntityAdded(int entityId)
        {
            if (_waterAreaMapper.Has(entityId)) // Check if the entity has a body
            {
                var waterArea = _waterAreaMapper.Get(entityId);
                LoadFishFromJson("Content/datas/Fish.json", waterArea);
                _world.AddWaterArea(waterArea);
            }
        }

        protected override void OnEntityRemoved(int entityId)
        {
            if (_waterAreaMapper.Has(entityId)) // Check if the entity has a body
            {
                var waterArea = _waterAreaMapper.Get(entityId);
                _world.RemoveWaterArea(waterArea);
            }
        }

        public void LoadFishFromJson(string path, WaterArea waterArea)
        {
            var json = File.ReadAllText(path);
            var fishDataList = JsonConvert.DeserializeObject<List<FishJsonData>>(json);

            foreach (var data in fishDataList)
            {
                foreach (var location in data.Location)
                {
                    if (location == _currentMap.Location.ToString())
                    {

                        var fish = new Fish()
                        {
                            Depth = data.Depth,
                            Difficult = data.Difficult,
                            SpawnChance = data.SpawnChance,
                            Diet = (Diet)Enum.Parse(typeof(Diet), data.Diet, true),
                            Behavior = data.Behavior,
                            Start = data.Start,
                            End = data.End,
                            Location = data.Location,
                        };
                        fish.MovementPattern = GenerateMovementPattern(fish.Behavior);
                        fish.CurrentMoveTime = (float)(_random.NextDouble()
                            * (fish.Behavior.MoveTimeMax
                            - fish.Behavior.MoveTimeMin)
                            + fish.Behavior.MoveTimeMin);

                        waterArea._fishData.Add(fish);

                        var item = new Item(data.Id, data.Name, data.Description);

                        waterArea._fishItem.Add(item);
                        break;
                    }
                }
            }
        }
        public List<Vector2> GenerateMovementPattern(FishBehaviorDefinition behavior)
        {
            var pattern = new List<Vector2>();
            float angleStep = MathF.Tau / behavior.Frequency;
            var extra = behavior.ExtraParams;

            switch (behavior.PatternType)
            {
                case PatternType.floater:
                    float xAmp = extra.Count > 0 ? extra[0] : 0.2f;
                    float yAmp = extra.Count > 1 ? extra[1] : 0.5f;
                    for (int i = 0; i <= behavior.Frequency; i++)
                    {
                        pattern.Add(new Vector2(MathF.Cos(i * angleStep) * xAmp, MathF.Sin(i * angleStep) * yAmp));
                    }
                    break;

                case PatternType.dart:
                    float dartSpeed = extra.Count > 0 ? extra[0] : 1.0f;
                    float boostChance = extra.Count > 1 ? extra[1] : 1.0f;
                    var directions = new List<Vector2>
                    {
                        Vector2.UnitX, -Vector2.UnitX,
                        Vector2.UnitY, -Vector2.UnitY
                    };

                    for (int i = 0; i < directions.Count; i++)
                    {
                        if (_random.NextDouble() < boostChance)
                        {
                            pattern.Add(directions[i] * dartSpeed);
                        }
                        pattern.Add(directions[i]);
                    }
                    break;

                case PatternType.dartMaster:
                    for (int i = 0; i < behavior.Frequency; i++)
                    {
                        dartSpeed = extra.Count > 0 ? extra[0] : 1.0f;
                        boostChance = extra.Count > 1 ? extra[1] : 1.0f;
                        float angle = (float)(_random.NextDouble() * MathF.Tau); // Random angle
                        if (_random.NextDouble() < boostChance)
                        {
                            pattern.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * dartSpeed);
                        }
                        pattern.Add(new Vector2(MathF.Cos(angle), MathF.Sin(angle)));
                    }
                    break;

                case PatternType.smooth:
                    float spiral = extra.Count > 0 ? extra[0] : 0.8f;
                    for (int i = 0; i < behavior.Frequency; i++)
                    {
                        pattern.Add(new Vector2(MathF.Sin(i * angleStep), MathF.Cos(i * angleStep)) * spiral);
                    }
                    break;

                case PatternType.intervMix:
                    var floatSteps = GenerateMovementPattern(new FishBehaviorDefinition
                    {
                        PatternType = PatternType.floater,
                        Frequency = behavior.Frequency,
                        ExtraParams = extra
                    });

                    var dartSteps = GenerateMovementPattern(new FishBehaviorDefinition
                    {
                        PatternType = PatternType.dart,
                        Frequency = behavior.Frequency,
                        ExtraParams = extra
                    });

                    // Interleave steps
                    int max = Math.Max(floatSteps.Count, dartSteps.Count);
                    for (int i = 0; i < max; i++)
                    {
                        if (i < floatSteps.Count) pattern.Add(floatSteps[i]);
                        if (i < dartSteps.Count) pattern.Add(dartSteps[i]);
                    }
                    break;

                case PatternType.probMix:
                    for (int i = 0; i < behavior.Frequency; i++)
                    {
                        bool useDart = _random.NextDouble() < 0.2; // 40% dart, 60% float

                        var tempBehavior = new FishBehaviorDefinition
                        {
                            PatternType = useDart ? PatternType.dart : PatternType.floater,
                            Frequency = behavior.Frequency,
                            ExtraParams = extra
                        };

                        var steps = GenerateMovementPattern(tempBehavior);
                        var step = steps[_random.Next(steps.Count)];
                        pattern.Add(step);
                    }
                    break;
            }
            return pattern;
        }
    }
}
