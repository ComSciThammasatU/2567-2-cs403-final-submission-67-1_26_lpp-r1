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
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;

namespace FinLeafIsle.Systems
{
    public class MapRenderSystem : DrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;
        private readonly ContentManager _content;
        private readonly ViewportAdapter _viewportAdapter;
        private GameState _gameState;
        private readonly Map _map;
        private readonly MapState _mapState;

        public MapRenderSystem(IContainer container)
        {
            _map = container.Resolve<Map>();
            _gameState = container.Resolve<GameState>();
            _spriteBatch = container.Resolve<SpriteBatch>();
            _content = container.Resolve<ContentManager>();
            _camera = container.Resolve<OrthographicCamera>();
            _mapState = container.Resolve<MapState>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
        }

        public override void Initialize(World world)
        {
            base.Initialize(world);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay || _gameState.State == GState.Playermenu)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());
                Texture2D pixel = _content.Load<Texture2D>("pixel");
                if (_mapState._state != MapLoaderState.Idle)
                {  
                        _spriteBatch.Draw(pixel,
                            new Vector2(0, 0),
                            new Rectangle(0, 0, 480, 270),
                            Color.Black,
                            0f,
                            Vector2.Zero,
                            1f,
                            SpriteEffects.None,
                            1f);
                       
                        
                }
                //_map.Draw(gameTime, _content, _spriteBatch, _camera);

                _spriteBatch.End();
            }
        }
    }
}
