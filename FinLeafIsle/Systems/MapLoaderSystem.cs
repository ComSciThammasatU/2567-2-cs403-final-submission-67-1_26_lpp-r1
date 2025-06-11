using Microsoft.Xna.Framework;
using Autofac;
using MonoGame.Extended.ECS.Systems;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using MonoGame.Extended;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.DayTimeWeather;
using FinLeafIsle.Datas;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinLeafIsle.Systems
{
    public class MapLoaderSystem : EntityUpdateSystem
    {

        private ContentManager _contentManager;
        private readonly GraphicsDevice _graphicsDevice;

        
        private GameWorld _world;
        private GameState _gameState;
        private MapLocation _currentLocation;
        private MapLocation _nextLocation;
        private MapLocation _bedLocation;
        private MapState _mapManager;
        private DayTime _dayTime;

        private ComponentMapper<WaterArea> _waterAreaMapper;
        private ComponentMapper<Wall> _wallMapper;
        private ComponentMapper<OnMap> _onMapMapper;

        private ComponentMapper<Fish> _fishMapper;
        private ComponentMapper<FishingRod> _fishingRodMapper;
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<Box> _boxMapper;
        private ComponentMapper<Bed> _bedMapper;

        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<Item> _itemMapper;
        private ComponentMapper<InventoryComponent> _inventoryComponentMapper;

        private ComponentMapper<AnimatedSprite> _animatedSpriteMapper;


        private Map _map;
        private string _tempPatch = "Content/datas/Temp/";
        private SaveManager _saveManager;

        public MapLoaderSystem(IContainer container)
        : base(Aspect.One(typeof(Wall), typeof(WaterArea), typeof(OnMap), typeof(Player)))
        {
            _world = container.Resolve<GameWorld>();
            _gameState = container.Resolve<GameState>();
            _map = container.Resolve<Map>();
            _currentLocation = container.ResolveNamed<MapLocation>("CurrentLocation");
            _nextLocation = container.ResolveNamed<MapLocation>("NextLocation");
            _bedLocation = container.ResolveNamed<MapLocation>("BedLocation");
            _mapManager = container.Resolve<MapState>();
            _contentManager = container.Resolve<ContentManager>();
            _graphicsDevice = container.Resolve<GraphicsDevice>();
            _saveManager = container.Resolve<SaveManager>();
            _dayTime = container.Resolve<DayTime>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _wallMapper = mapperService.GetMapper<Wall>();
            _waterAreaMapper = mapperService.GetMapper<WaterArea>();
            _boxMapper = mapperService.GetMapper<Box>();
            _itemMapper = mapperService.GetMapper<Item>();
            _inventoryComponentMapper = mapperService.GetMapper<InventoryComponent>();
            _onMapMapper = mapperService.GetMapper<OnMap>();
            _fishMapper = mapperService.GetMapper<Fish>();
            _fishingRodMapper = mapperService.GetMapper<FishingRod>();
            _playerMapper = mapperService.GetMapper<Player>();
            _animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _bedMapper = mapperService.GetMapper<Bed>();
        }

        public override void Update(GameTime gameTime)
        {

            var json = new string("");
            var mapPath = new string("");
            var playerPath = new string("");
            var gameDataPath = new string("");
            //SaveMapState
            switch (_mapManager._state)
            {
                case (MapLoaderState.Unload):

                    if (_map != null)
                    {
                        foreach (var entity in ActiveEntities)
                        {
                            if (_wallMapper.Has(entity)) // Destoy Wall
                            {
                                DestroyEntity(entity);
                                continue;
                            }
                            if (_waterAreaMapper.Has(entity)) // Destoy WaterArea
                            {
                                DestroyEntity(entity);
                                continue;
                            }
                            if (_playerMapper.Has(entity))
                            {
                                DestroyEntity(entity);
                                continue;
                            }

                            DestroyEntity(entity);

                        }
                    }

                    _dayTime.Day = 1;

                    _world._staticBodies.Clear();
                    _world._dynamicBodies.Clear();
                    _world._waterAreas.Clear();
                    _world._gateAreas.Clear();
                    _world._cameraBlocks.Clear();
                    _world._beds.Clear();

                    _mapManager._state = MapLoaderState.Idle;
                    break;

                case (MapLoaderState.SaveAndUnload):
                    var playerData = new SavedEntityData();

                    var mapData = new SavedMapData
                    {
                        MapName = _currentLocation.Location.ToString()
                    };

                    if (_map != null)
                    {
                        foreach (var entity in ActiveEntities)
                        {
                            if (_wallMapper.Has(entity)) // Destoy Wall
                            {
                                DestroyEntity(entity);
                                continue;
                            }
                            if (_waterAreaMapper.Has(entity)) // Destoy WaterArea
                            {
                                DestroyEntity(entity);
                                continue;
                            }
                            if (_playerMapper.Has(entity))
                            {
                                playerData = SaveEntity(entity);
                                DestroyEntity(entity);
                                continue;
                            }

                            var EntityData = SaveEntity(entity);
                            mapData.Entities.Add(EntityData);
                            DestroyEntity(entity);

                        }
                    }
                    var gameData = new SavedGameData();
                    gameData.Day = _dayTime.Day;
                    gameData.BedLocation = _bedLocation;

                    Directory.CreateDirectory(_tempPatch + "mapData/");
                    //Write Map
                    json = JsonConvert.SerializeObject(mapData, Formatting.Indented);
                    mapPath = Path.Combine(_tempPatch + "mapData/", _currentLocation.Location.ToString() + ".json");
                    File.WriteAllText(mapPath, json);
                    //Write Player
                    json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
                    playerPath = Path.Combine(_tempPatch + "playerData.json");
                    File.WriteAllText(playerPath, json);
                    //Write GameData
                    json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                    gameDataPath = Path.Combine(_tempPatch + "gameData.json");
                    File.WriteAllText(gameDataPath, json);
                    _world._staticBodies.Clear();
                    _world._dynamicBodies.Clear();
                    _world._waterAreas.Clear();
                    _world._gateAreas.Clear();
                    _world._cameraBlocks.Clear();
                    _world._beds.Clear();

                    if (_gameState.State == GState.GamePlay)
                        _mapManager._state = MapLoaderState.LoadMap;
                    else
                    {
                        _saveManager.CopyTempToSaveSlot();
                        _mapManager._state = MapLoaderState.Idle;
                    }
                        
                    break;

                case (MapLoaderState.LoadMap):
                    _currentLocation.Location = _nextLocation.Location;
                    _currentLocation.Target = _nextLocation.Target;

                    gameDataPath = Path.Combine(_tempPatch + "gameData.json");
                    if (File.Exists(gameDataPath))
                    {
                        json = File.ReadAllText(gameDataPath);
                        gameData = JsonConvert.DeserializeObject<SavedGameData>(json);
                        _dayTime.Day = gameData.Day;
                        _bedLocation.Location = gameData.BedLocation.Location;
                        _bedLocation.Target = gameData.BedLocation.Target;
                        System.Diagnostics.Debug.WriteLine($"Found Ya!!!  {_bedLocation.Location}");
                    }


                    playerPath = Path.Combine(_tempPatch + "playerData.json");
                    if (File.Exists(playerPath))
                    {
                        json = File.ReadAllText(playerPath);
                        playerData = JsonConvert.DeserializeObject<SavedEntityData>(json);
                        LoadEntity(playerData);
                    }
                    else
                    {
                        GameMain._entityFactory.CreatePlayer(_currentLocation.Target);
                    }

                    _map.LoadMap(_contentManager, _graphicsDevice, _currentLocation, _world);

                    mapPath = Path.Combine(_tempPatch + "mapData/", _currentLocation.Location.ToString() + ".json");

                    if (File.Exists(mapPath))
                    {
                        json = File.ReadAllText(mapPath);
                        mapData = JsonConvert.DeserializeObject<SavedMapData>(json);

                        foreach (var data in mapData.Entities)
                        {
                            LoadEntity(data);
                        }
                    }

                    
                    _mapManager._state = MapLoaderState.LoadFinish;
                    break;

                case (MapLoaderState.LoadFinish):
                    _mapManager._state = MapLoaderState.Idle;
                    break;


                   
            }
        }
        private SavedEntityData SaveEntity(int entity)
        {
            var EntityData = new SavedEntityData();

            if (_playerMapper.Has(entity))
                EntityData.Player = _playerMapper.Get(entity);
            if (_boxMapper.Has(entity))
                EntityData.Box = _boxMapper.Get(entity);
            if (_fishMapper.Has(entity))
                EntityData.Fish = _fishMapper.Get(entity);
            if (_fishingRodMapper.Has(entity))
                EntityData.FishingRod = _fishingRodMapper.Get(entity);
            if (_bedMapper.Has(entity))
                EntityData.Bed = _bedMapper.Get(entity);

            if (_itemMapper.Has(entity))
                EntityData.Item = _itemMapper.Get(entity);
            if (_bodyMapper.Has(entity))
                EntityData.Body = _bodyMapper.Get(entity);
            if (_transformMapper.Has(entity))
                EntityData.Transform = _transformMapper.Get(entity);

            if (_inventoryComponentMapper.Has(entity))
            {
                var inventory = _inventoryComponentMapper.Get(entity);
                for (int i = 0; i < inventory._inventorySlot.Count; i++)
                {
                    var slot = inventory._inventorySlot[i];

                    if (slot._item != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Found Ya!!! Item that need contain");
                        var EntityDataC = SaveEntity(slot._item);
                        EntityDataC.ContainSlot = i.ToString();
                        EntityData.ContainItem.Add(EntityDataC);
                        slot._item.Destroy();
                    }

                }
            }
            return EntityData;
        }
        private SavedEntityData SaveEntity(Entity entity)
        {
            var EntityData = new SavedEntityData();

            if (entity.Has<Box>())
                EntityData.Box = entity.Get<Box>();
            if (entity.Has<Fish>())
                EntityData.Fish = entity.Get<Fish>();
            if (entity.Has<FishingRod>())
                EntityData.FishingRod = entity.Get<FishingRod>();

            if (entity.Has<Item>())
                EntityData.Item = entity.Get<Item>();
            if (entity.Has<Body>())
                EntityData.Body = entity.Get<Body>();
            if (entity.Has<Transform2>())
                EntityData.Transform = entity.Get<Transform2>();

            return EntityData;
        }

        private Entity LoadEntity(SavedEntityData data)
        {
            if (data.Player != null)
            {
                Player player = data.Player;
                Transform2 transform = data.Transform;
                Body body = data.Body;
                body.Position = _nextLocation.Target;

                InventoryComponent inventory = new InventoryComponent(36);
                foreach (var entity in data.ContainItem)
                {
                    var dataC = LoadEntity(entity);
                    int slot = Convert.ToInt32(entity.ContainSlot);
                    inventory._inventorySlot[slot]._item = dataC;
                }
                return GameMain._entityFactory.CreatePlayer(transform, body, player, inventory);
            }

            if (data.Box != null)
            {
                Item item = data.Item;
                Transform2 transform = data.Transform;
                Body body = data.Body;
                InventoryComponent inventory = new InventoryComponent(36);
                foreach (var entity in data.ContainItem)
                {
                    var dataC = LoadEntity(entity);
                    int slot = Convert.ToInt32(entity.ContainSlot);
                    inventory._inventorySlot[slot]._item = dataC;
                }
                return GameMain._entityFactory.CreateBox(item, inventory, transform, body);
            }
            if (data.Fish != null)
            {
                Item item = data.Item;
                Fish fish = data.Fish;
                return GameMain._entityFactory.CreateFish(item, fish);
            }
            if (data.FishingRod != null)
            {
                Item item = data.Item;
                FishingRod fishingRod = data.FishingRod;
                return GameMain._entityFactory.CreateFishingRod(item, fishingRod);
            }
            if (data.Bed != null)
            {

                return GameMain._entityFactory.CreateBed(data.Bed.Position, data.Bed.Size);
            }
            return null;
        }

    }
}
