using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using Autofac;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.DayTimeWeather;

namespace FinLeafIsle.Systems
{
    public class GateSystem : EntityProcessingSystem
    {
        private GameWorld _world;
        private MapState _mapState;
        private MapLocation _nextMap;
        private MapLocation _currentMap;
        private MapLocation _bedLocation;
        private GameState _gameState;
        private SaveDayPage _saveDayPage;
        private DayTime _dayTime;

        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;

        public GateSystem(IContainer container, SaveDayPage saveDayPage)
            : base(Aspect.All(typeof(Player)))
        {
            _saveDayPage = saveDayPage;
            _world = container.Resolve<GameWorld>();
            _mapState = container.Resolve<MapState>();
            _nextMap = container.ResolveNamed<MapLocation>("NextLocation");
            _currentMap = container.ResolveNamed<MapLocation>("CurrentLocation");
            _bedLocation = container.ResolveNamed<MapLocation>("BedLocation");
            _gameState = container.Resolve<GameState>();
            _dayTime = container.Resolve<DayTime>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_gameState.State == GState.GamePlay)
            {
                var body = _bodyMapper.Get(entityId);
                var transform = _transformMapper.Get(entityId);

                foreach (var gate in _world._gateAreas)
                {
                    if (CollisionTester.AabbAabb(body.BoundingBox, gate.BoundingBox))
                    {
                        _nextMap.Location = Enum.Parse<Location>(gate.Destination);
                        _nextMap.Target = gate.Target;
                        _mapState._state = MapLoaderState.SaveAndUnload;
                        
                    }
                }

                foreach (var bed in _world._beds)
                {
                    if (CollisionTester.AabbAabb(body.BoundingBox, bed.BoundingBox))
                    {
                        _nextMap.Location = _currentMap.Location;
                        _nextMap.Target = bed.Position - new Vector2(32, 0);
                        _bedLocation.Location = _nextMap.Location;
                        _bedLocation.Target = _nextMap.Target;
                        _dayTime.Day += 1;
                        _saveDayPage.Saved();

                    }
                }
                if (_dayTime.Time >= 2110)
                {
                    _nextMap.Location = _bedLocation.Location;
                    _nextMap.Target = _bedLocation.Target;
                    _dayTime.Day += 1;
                    _saveDayPage.Saved();
                }
            }
        }
    }
}
