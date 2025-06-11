using Autofac;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.Components.MiniGame;
using FinLeafIsle.DayTimeWeather;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FinLeafIsle.Systems
{
    public class FishingSystem : EntityProcessingSystem
    {
        private GameWorld _world;
        private GameState _gameState;
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<AnimatedSprite> _animatedSpriteMapper;
        private ComponentMapper<Sprite> _spriteMapper;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<InventoryComponent> _inventoryMapper;
        private ComponentMapper<FishingComponent> _fishingMapper;
        private ComponentMapper<FishingMiniGameComponent> _fishingMiniGameMapper;
        private InventorySlot _handSlot;
        private MapLocation _currentLocation;
        private Random _random;
        private AudioManager _audioManager;
        private DayTime _dayTime;

        public FishingSystem(IContainer container)
        : base(Aspect.All(typeof(Player), typeof(FishingComponent)))
        {
            _world = container.Resolve<GameWorld>();
            _gameState = container.Resolve<GameState>();
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
            _currentLocation = container.ResolveNamed<MapLocation>("CurrentLocation");
            _random = container.Resolve<Random>();
            _audioManager = container.Resolve<AudioManager>();
            _dayTime = container.Resolve<DayTime>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _spriteMapper = mapperService.GetMapper<Sprite>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _inventoryMapper = mapperService.GetMapper<InventoryComponent>();
            _fishingMapper = mapperService.GetMapper<FishingComponent>();
            _playerMapper = mapperService.GetMapper<Player>();
            _fishingMiniGameMapper = mapperService.GetMapper<FishingMiniGameComponent>();

            
            _audioManager._chargeS.IsLooped = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay)
            {
                base.Update(gameTime);

            }
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_gameState.State == GState.GamePlay)
            {
                var player = _playerMapper.Get(entityId);
                var sprite = _spriteMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);
                var body = _bodyMapper.Get(entityId);
                var fishing = _fishingMapper.Get(entityId);
                var inventory = _inventoryMapper.Get(entityId);
                var mouseState = MouseExtended.GetState();

                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


                //Fishing state handle
                switch (fishing.State)
                {
                    case FishingState.None:
                        if (mouseState.WasButtonPressed(MouseButton.Left))
                        {
                            player.State = State.Fishing;
                            fishing.HookCurrentPosition = transform.Position;
                            fishing.State = FishingState.Charging;
                            fishing.Timer = 0f;
                        }
                        break;

                    case FishingState.Charging:
                        if (mouseState.IsButtonDown(MouseButton.Left))
                        {
                            if (_handSlot._item.Has<FishingRod>())
                            {
                                FishingRod rod = _handSlot._item.Get<FishingRod>(); 
                                fishing.Timer += dt;
                                fishing.CastDistance = GetChargedDistance(fishing.Timer, rod); // Sine curve or ping-pong;
                            }
                            
                            if (_audioManager._chargeS.State != SoundState.Playing)
                            {
                                _audioManager._chargeS.Play();
                            }
                        }
                        else if (mouseState.IsButtonUp(MouseButton.Left))
                        {
                            fishing.HookStartPosition = body.Position;
                            fishing.HookTargetPosition = GetCastTargetPosition(player.Facing, body.Position, fishing.CastDistance);
                            fishing.HookCurrentPosition = fishing.HookStartPosition;
                            fishing.HookTravelProgress = 0f;
                            fishing.State = FishingState.MovingHook;

                            if (_audioManager._chargeS.State == SoundState.Playing)
                            {
                                _audioManager._chargeS.Stop();
                            }
                            _audioManager._castS.Play();
                        }
                        break;

                    case FishingState.MovingHook:
                        fishing.HookTravelProgress += dt / 0.5f;

                        if (fishing.HookTravelProgress >= 1f)
                        {
                            fishing.HookCurrentPosition = fishing.HookTargetPosition;
                            fishing.HookTravelProgress = 1f;
                            fishing.State = FishingState.GetFish;
                            fishing.FishBiteDelay = RandomFishBiteDelay();
                            fishing.Timer = 0f;
                            System.Diagnostics.Debug.WriteLine("Hook casted at distance: " + fishing.HookCurrentPosition);
                        }
                        else
                        {
                            fishing.HookCurrentPosition = Vector2.Lerp(
                                fishing.HookStartPosition,
                                fishing.HookTargetPosition,
                                fishing.HookTravelProgress
                            );
                        }
                        break;

                    case FishingState.GetFish:
                        fishing.FishOnHook = GetRandomFish(fishing.HookCurrentPosition);
                        if (fishing.FishOnHook == null)
                        {
                            fishing.State = FishingState.CancelFishing;
                            break;
                        }
                        fishing.State = FishingState.WaitingForFish;
                        break;

                    case FishingState.WaitingForFish:

                        fishing.Timer += dt;
                        if (fishing.Timer > fishing.FishBiteDelay)
                        {

                            fishing.Timer = 0f;
                            fishing.State = FishingState.FishOnHook;

                        }
                        break;

                    case FishingState.FishOnHook:
                        _audioManager._hookedS.Play();
                        _fishingMiniGameMapper.Put(entityId,
                            new FishingMiniGameComponent
                            {
                                Center = fishing.HookCurrentPosition,
                                BigCircleRadius = 32f,
                                SmallCircleRadius = 12f,
                            });
                        fishing.State = FishingState.Minigame;

                        break;

                    case FishingState.Minigame:
                        break;

                    case FishingState.CaughtFish:
                        _fishingMiniGameMapper.Delete(entityId);
                        _fishingMapper.Delete(entityId);
                        inventory.AddItem(fishing.FishOnHook);
                        fishing.FishOnHook = null;
                        fishing.State = FishingState.None;
                        player.State = State.Idle;
                        break;

                    case FishingState.CancelFishing:
                        _fishingMiniGameMapper.Delete(entityId);
                        _fishingMapper.Delete(entityId);
                        if(fishing.FishOnHook != null)
                            fishing.FishOnHook.Destroy();
                        fishing.State = FishingState.None;
                        player.State = State.Idle;
                        break;

                }
                /*
                DebugOverlay.AddLine($"_[Fishing]");
                DebugOverlay.AddLine($"Hook Pos: {fishing.HookCurrentPosition}");
                DebugOverlay.AddLine($"Charge: {fishing.CastDistance}");
                */
            }
        }
        private float GetChargedDistance(float timer, FishingRod rod)
        {
            float min = 32f;
            float max = rod.MaxCastingDistance;
            float percent = (timer / rod.MaxChargeTime) % 2f; // Loop 0 → 2
            float pingPong = percent > 1f ? 2f - percent : percent; // PingPong effect between 0 → 1 → 0

            float pitch = MathHelper.Lerp(-0.5f, 0.5f, pingPong);
            _audioManager._chargeS.Pitch = pitch;

            return MathHelper.Lerp(min, max, pingPong);
        }

        private Vector2 GetCastTargetPosition(Facing facing, Vector2 start, float distance)
        {
            return facing switch
            {
                Facing.Left => start + new Vector2(-distance, 0),
                Facing.Right => start + new Vector2(distance, 0),
                Facing.Up => start + new Vector2(0, -distance),
                Facing.Down => start + new Vector2(0, distance),
                _ => start
            };
        }

        private float RandomFishBiteDelay()
        {
            return (float)(_random.NextDouble() * (4.0 - 1.5) + 1.5);
        }

        private Entity GetRandomFish(Vector2 position)
        {
            var listWater = _world._waterAreas;

            foreach (var waterArea in listWater)
            {

                if (waterArea.BoundingBox.Contain(position))
                {

                    var fishList = new List<Fish>();
                    var itemList = new List<Item>();
                    
                    for (int i = 0;i < waterArea._fishData.Count; i++)
                    {
                        if ( _dayTime.Time >= waterArea._fishData[i].Start && _dayTime.Time <= waterArea._fishData[i].End)
                        {
                            fishList.Add( waterArea._fishData[i] );
                            itemList.Add(waterArea._fishItem[i]);
                        }
                     
                    }
                      
                    if (fishList == null || fishList.Count == 0) return null;

                    float totalWeight = fishList.Sum(f => f.SpawnChance);
                    float roll = (float)(_random.NextDouble() * totalWeight);

                    float cumulative = 0f;

                    for (int i = 0; i < fishList.Count; i++)
                    {
                        cumulative += fishList[i].SpawnChance;
                        if (roll <= cumulative)
                        {
                            var fish = fishList[i];
                            var item = itemList[i];

                            return GameMain._entityFactory.CreateFish(item, fish);
                        }
                    }
                }
            }
            return null;
        }
    }
}
