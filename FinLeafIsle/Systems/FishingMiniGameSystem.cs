using Autofac;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.Components.MiniGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Diagnostics;

namespace FinLeafIsle.Systems
{
    public class FishingMiniGameSystem : EntityProcessingSystem
    {
        private GameState _gameState;
        private InventorySlot _handSlot;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<FishingComponent> _fishingMapper;
        private ComponentMapper<FishingMiniGameComponent> _fishingMiniGameMapper;
        private ComponentMapper<FishingRod> _fishMapper;
        private Random _random;
        private AudioManager _audioManager;
        
        public FishingMiniGameSystem(IContainer container)
            : base(Aspect.All(typeof(Player), typeof(FishingMiniGameComponent)))
        {

            _gameState = container.Resolve<GameState>();
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
            _random = container.Resolve<Random>();
            _audioManager = container.Resolve<AudioManager>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _playerMapper = mapperService.GetMapper<Player>();
            _fishingMapper = mapperService.GetMapper<FishingComponent>();
            _fishingMiniGameMapper = mapperService.GetMapper<FishingMiniGameComponent>();
            _fishMapper = mapperService.GetMapper<FishingRod>();

            
            _audioManager._reelInS.IsLooped = true;
            _audioManager._reelOutS.IsLooped = true;
            _audioManager._progessS.IsLooped = true;
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_gameState.State == GState.GamePlay)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var transform = _transformMapper.Get(entityId);
                var body = _bodyMapper.Get(entityId);
                var fishing = _fishingMapper.Get(entityId);
                var fishingMiniGame = _fishingMiniGameMapper.Get(entityId);

                var keyboardState = KeyboardExtended.GetState();

                //MovementLogic


                Vector2 directionToMove = Vector2.Zero;

                var fish = fishing.FishOnHook.Get<Fish>();
                fish.MoveTimer += dt;
                fish.TiredTimer += dt;
                if (fish.MoveTimer >= fish.CurrentMoveTime)
                {
                    fish.CurrentPattern = (fish.CurrentPattern + 1) % fish.MovementPattern.Count;
                    fish.MoveTimer = 0f;
                    fish.CurrentMoveTime = (float)(_random.NextDouble()
                    * (fish.Behavior.MoveTimeMax
                    - fish.Behavior.MoveTimeMin)
                    + fish.Behavior.MoveTimeMin);
                }
                // Check if fish gets tired
                if (fish.TiredTimer >= fish.Behavior.TiredTime)
                {
                    fish.IsTired = true;
                    fish.TiredTimer = 0f;
                }


                if (fish.IsTired)
                {
                    if (fish.TiredTimer >= fish.Behavior.TiredDuration)
                    {
                        fish.IsTired = false;
                        fish.TiredTimer = 0f;
                    }
                }

                float strengthThisFrame = fish.IsTired
                    ? fish.Behavior.Strength * fish.Behavior.TiredStrengthMultiplier
                    : fish.Behavior.Strength;

                Vector2 fishMovement = fish.MovementPattern[fish.CurrentPattern] * strengthThisFrame * 30;

                Vector2 playerInput = Vector2.Zero;
                if (keyboardState.IsKeyDown(Keys.W)) playerInput.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.S)) playerInput.Y += 1;
                if (keyboardState.IsKeyDown(Keys.A)) playerInput.X -= 1;
                if (keyboardState.IsKeyDown(Keys.D)) playerInput.X += 1;
                if (playerInput != Vector2.Zero)
                    playerInput.Normalize();

                Vector2 playerMovement = Vector2.Zero;
                if (_handSlot._item.Has<FishingRod>())
                {
                    FishingRod rod = _handSlot._item.Get<FishingRod>();
                    playerMovement = playerInput * rod.Strength * 30;
                }

                
                    float newDistanceToCenter = Vector2.Distance(fishing.HookCurrentPosition, fishingMiniGame.Center);
                    float delta = newDistanceToCenter - fishingMiniGame.LastDistanceToCenter;

                    fishingMiniGame.LastDistanceToCenter = newDistanceToCenter;

                    if (delta < 0)
                    {
                    // Moving toward center — player is winning
                    _audioManager._reelInS.Play();
                    _audioManager._reelOutS.Stop();
                    }
                    else
                    {
                    // Moving away from center — fish is pulling
                    _audioManager._reelInS.Stop();
                    _audioManager._reelOutS.Play();
                    }
                



                directionToMove = fishMovement + playerMovement;

                fishing.HookCurrentPosition += directionToMove * dt;

                //CircleLogic
                float distanceToCenter = Vector2.Distance(fishing.HookCurrentPosition, fishingMiniGame.Center);
                float divPro = fishingMiniGame.Progress / 50;
                float safePitch = MathHelper.Lerp(0f, 1f, divPro);

                _audioManager._progessS.Pitch = MathHelper.Clamp(safePitch, -1f, 1f);
                
                if (distanceToCenter < fishingMiniGame.SmallCircleRadius)
                {
                    fishingMiniGame.Progress += dt * fishingMiniGame.ProgressSpeed;
                    _audioManager._progessS.Play();
                }
                else if (distanceToCenter > fishingMiniGame.BigCircleRadius)
                {
                    fishingMiniGame.Progress = 0;
                    fishing.State = FishingState.CancelFishing;
                    _audioManager._reelInS.Stop();
                    _audioManager._reelOutS.Stop();
                    _audioManager._progessS.Stop();
                    _audioManager._castS.Play();
                }
                else
                {
                    fishingMiniGame.Progress -= dt * fishingMiniGame.DecaySpeed;
                    fishingMiniGame.Progress = Math.Max(0, fishingMiniGame.Progress);
                    //fishingMiniGame.Progress = MathHelper.Clamp(fishingMiniGame.Progress, 0, 1f);
                    _audioManager._progessS.Stop();
                }
                //Success
                if (fishingMiniGame.Progress >= fish.Difficult)
                {
                    fishingMiniGame.Progress = 0;
                    fishing.State = FishingState.CaughtFish;
                    _audioManager._reelInS.Stop();
                    _audioManager._reelOutS.Stop();
                    _audioManager._progessS.Stop();
                    _audioManager._catchS.Play();
                }
                /*
                 DebugOverlay.AddLine($"_[FishingMiniGame]");
                 DebugOverlay.AddLine($"Delta move: {delta}");
                 DebugOverlay.AddLine($"FishOnHook: {fishing.FishOnHook.Name}");
                 DebugOverlay.AddLine($"FishType: {fishing.FishOnHook.Behavior.PatternType}");
                 DebugOverlay.AddLine($"Fish Move: {fishMovement}");
                 DebugOverlay.AddLine($"Fish Move Count: {fishing.FishOnHook.CurrentPattern}");
                 DebugOverlay.AddLine($"FishIsT: {fishing.FishOnHook.IsTired}");
                 DebugOverlay.AddLine($"Player Move: {playerMovement}");
                 DebugOverlay.AddLine($"Progress: {fishingMiniGame.Progress:0.00}");
                 DebugOverlay.AddLine($"Pattern Index: {fishing.FishOnHook.CurrentPattern}");
                 DebugOverlay.AddLine($"Distance to Center: {Vector2.Distance(fishing.HookCurrentPosition, fishingMiniGame.Center):0.00}");
                 DebugOverlay.AddLine($"State: {fishing.State}");
                 */
            }
        }
    }
}
