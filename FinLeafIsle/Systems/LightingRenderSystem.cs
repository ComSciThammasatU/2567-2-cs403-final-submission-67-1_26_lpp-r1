using FinLeafIsle.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using Autofac;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Input;
using FinLeafIsle.DayTimeWeather;

namespace FinLeafIsle.Systems
{
    public class LightingRenderSystem : EntityDrawSystem
    {
        private readonly GameState _gameState;
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;
        private readonly ContentManager _content;
        private readonly ViewportAdapter _viewportAdapter;
        private ComponentMapper<Transform2> _transformMapper;
        private DayTime _dayTime;


        public LightingRenderSystem(IContainer container)
         : base(Aspect.All(typeof(Player)))
        {
            _spriteBatch = container.Resolve<SpriteBatch>();
            _camera = container.Resolve<OrthographicCamera>();
            _content = container.Resolve<ContentManager>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
            _dayTime = container.Resolve<DayTime>();
            _gameState = container.Resolve<GameState>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay)
            {
                foreach (var entityId in ActiveEntities)
                {
                    var transform = _transformMapper.Get(entityId);

                    _spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());
                    //Draw Overlay Light
                    Color currentTint = _dayTime.GetCurrentTint(_dayTime.Time);
                    Texture2D pixel = _content.Load<Texture2D>("pixel");
                    _spriteBatch.Draw(pixel,
                        new Vector2(0, 0),
                        new Rectangle(0, 0, 480, 270),
                        currentTint,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);

                    _spriteBatch.End();

                    if (_dayTime.Time > 1930)
                    {
                        _spriteBatch.Begin(blendState: BlendState.Additive, samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());
                        Texture2D radial = _content.Load<Texture2D>("Light/radial");
                        Vector2 maskOrigin = new Vector2(radial.Width / 2f, radial.Height / 2f);

                        //Draw Overlay Light
                        _spriteBatch.Draw(radial, transform.Position, null, new Color(255, 157, 36) * 0.5f, 0f, maskOrigin, 0.3f, SpriteEffects.None, 0f);

                        _spriteBatch.End();
                    }
                }
            }
        }
    }
}
