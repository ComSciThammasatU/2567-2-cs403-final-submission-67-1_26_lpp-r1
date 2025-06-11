using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using MonoGame.Extended.Input;
using Autofac;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.Components.MiniGame;

namespace FinLeafIsle.Systems
{
    public class CameraSystem : EntityProcessingSystem
    {
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<FishingComponent> _fishingMapper;
        private ComponentMapper<FishingMiniGameComponent> _fishingMiniGameMapper;
        private ComponentMapper<CameraBlock> _cameraBlockMapper;

        private readonly OrthographicCamera _camera;
        private readonly GameWorld _world;

        public CameraSystem(IContainer container)
            : base(Aspect.All(typeof(Body), typeof(Player), typeof(Transform2)))
        {
            _camera = container.Resolve<OrthographicCamera>();
           _world = container.Resolve<GameWorld>();
        }
        public override void Initialize(IComponentMapperService mapperService)
        {
            _playerMapper = mapperService.GetMapper<Player>();           
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _fishingMapper = mapperService.GetMapper<FishingComponent>();
            _fishingMiniGameMapper = mapperService.GetMapper<FishingMiniGameComponent>();
            _cameraBlockMapper = mapperService.GetMapper<CameraBlock>();
        }
        /*
        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                var player = _playerMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);
                var body = _bodyMapper.Get(entityId);

                _camera.LookAt(body.Position);
            }  
        }
        */

        public override void Process(GameTime gameTime, int entityId)
        {
            var keyboardState = KeyboardExtended.GetState();

            if (keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.OemMinus))
            {
                _camera.ZoomOut(0.01f);
            }
            if (keyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyDown(Keys.OemPlus))
            {
                _camera.ZoomIn(0.01f);
            }

            var player = _playerMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);
            var body = _bodyMapper.Get(entityId);
            var fishing = _fishingMapper.Get(entityId);
            var cameraBlock = _cameraBlockMapper.Get(entityId);

            if (player.State != State.Fishing)
                _camera.LookAt(cameraBlock.Position);
            else
            {
                if (_fishingMiniGameMapper.Has(entityId))
                {
                    var fishingMiniGame = _fishingMiniGameMapper.Get(entityId);
                    _camera.LookAt(fishingMiniGame.Center);
                }
                else
                {
                    if (_fishingMapper.Has(entityId))
                    _camera.LookAt(fishing.HookCurrentPosition);
                }
            }

            // Compute desired movement vector first
            Vector2 desiredMove = body.Position - cameraBlock.Position;
            cameraBlock.Position += desiredMove;
            foreach (var cb in _world._cameraBlocks)
            {
                
                var vector = cb.Position - cameraBlock.Position;

                if (CollisionTester.AabbAabb(cameraBlock.BoundingBox, cb.BoundingBox, vector, out var manifold))
                {
                    cameraBlock.Position -= manifold.Normal * manifold.Penetration;
                    cameraBlock.Velocity = cameraBlock.Velocity * new Vector2(Math.Abs(manifold.Normal.Y), Math.Abs(manifold.Normal.X));
                    
                }
                
            }
        }

    }
}
