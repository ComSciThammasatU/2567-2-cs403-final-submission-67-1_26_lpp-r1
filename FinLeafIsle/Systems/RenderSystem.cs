using FinLeafIsle.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using Autofac;
using FinLeafIsle.Components;

namespace FinLeafIsle.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;
        private ComponentMapper<AnimatedSprite> _animatedSpriteMapper;
        private ComponentMapper<Sprite> _spriteMapper;
        private ComponentMapper<Transform2> _transforMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<Offset> _offsetMapper;
        private GameState _gameState;

        public RenderSystem(IContainer container)
         : base(Aspect.All(typeof(Transform2)).One(typeof(AnimatedSprite), typeof(Sprite)))
        {
            _spriteBatch = container.Resolve<SpriteBatch>();
            _camera = container.Resolve<OrthographicCamera>();
            _gameState = container.Resolve<GameState>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transforMapper = mapperService.GetMapper<Transform2>();
            _animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _spriteMapper = mapperService.GetMapper<Sprite>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _offsetMapper = mapperService.GetMapper<Offset>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

                foreach (var entity in ActiveEntities)
                {
                    var sprite = _animatedSpriteMapper.Has(entity)
                        ? _animatedSpriteMapper.Get(entity)
                        : _spriteMapper.Get(entity);
                    var transform = _transforMapper.Get(entity);

                    if (sprite is AnimatedSprite animatedSprite)
                        animatedSprite.Update(gameTime);

                    var offset = _offsetMapper.Get(entity);
                    sprite.Origin = new Vector2(sprite.TextureRegion.Width - offset.offset.X, sprite.TextureRegion.Height - offset.offset.Y);

                    //System.Diagnostics.Debug.WriteLine($"X :{sprite.Origin.X}, Y :{sprite.Origin.Y}");
                    _spriteBatch.Draw(sprite, transform.Position, 0f, Vector2.One);

                }

                _spriteBatch.End();

            }
        }
    }
}
