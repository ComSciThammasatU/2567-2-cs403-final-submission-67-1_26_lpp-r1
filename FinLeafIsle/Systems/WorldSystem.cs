using Autofac;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;


namespace FinLeafIsle.Systems
{
    public class WorldSystem : EntityProcessingSystem
    {   
        private GameState _gameState;
        private readonly GameWorld _world;
        private Body _body;

        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<Player> _playerMapper;
        

        public WorldSystem(IContainer container)
            : base(Aspect.All(typeof(Body), typeof(Transform2)))
        {
            _world = container.Resolve<GameWorld>();
            _gameState = container.Resolve<GameState>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _playerMapper = mapperService.GetMapper<Player>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            if (_bodyMapper.Has(entityId)) // Check if the entity has a body
            {
                var body = _bodyMapper.Get(entityId);
                if (_playerMapper.Has(entityId))
                    _body = body;
                _world.AddBody(body);
            }
        }

        protected override void OnEntityRemoved(int entityId)
        {
            if (_bodyMapper.Has(entityId)) // Check if the entity has a body
            {
                var body = _bodyMapper.Get(entityId);
                _world.RemoveBody(body);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay)
            {
                base.Update(gameTime);
                _world.Update(gameTime.GetElapsedSeconds());
            }
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_gameState.State == GState.GamePlay)
            {
                var transform = _transformMapper.Get(entityId);
                var body = _bodyMapper.Get(entityId);
                transform.Position = body.Position;
            }
        }
    }
}
