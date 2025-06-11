using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;

namespace FinLeafIsle.Systems
{
    public class BodyRenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;
        
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<WaterArea> _waterAreaMapper;
        private readonly ContentManager _contentManager;

        public BodyRenderSystem(SpriteBatch spriteBatch, OrthographicCamera camera, ContentManager contentManager)
        : base(Aspect.All(typeof(Body)))
        {
            _spriteBatch = spriteBatch;
            _camera = camera;
            _contentManager = contentManager;

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            
            _bodyMapper = mapperService.GetMapper<Body>();
            _waterAreaMapper = mapperService.GetMapper<WaterArea>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

            foreach (var entity in ActiveEntities)
            {
                Texture2D _texture = _contentManager.Load<Texture2D>("pixel");

                var body = _bodyMapper.Get(entity);
               /*var waterArea = _waterAreaMapper.Get(entity);
                

                if (waterArea.Depth == 0 )
                {
                    _spriteBatch.Draw(_texture, waterArea.BoundingBox.Min, new Rectangle((int)waterArea.BoundingBox.Min.X, (int)waterArea.BoundingBox.Min.Y, (int)waterArea.BoundingBox.Width, (int)waterArea.BoundingBox.Height), Color.DarkGray);
                }
                else if (waterArea.Depth == 1 )
                {
                    _spriteBatch.Draw(_texture, waterArea.BoundingBox.Min, new Rectangle((int)waterArea.BoundingBox.Min.X, (int)waterArea.BoundingBox.Min.Y, (int)waterArea.BoundingBox.Width, (int)waterArea.BoundingBox.Height), Color.Gray);
                }
                else
                {
                    _spriteBatch.Draw(_texture, waterArea.BoundingBox.Min, new Rectangle((int)waterArea.BoundingBox.Min.X, (int)waterArea.BoundingBox.Min.Y, (int)waterArea.BoundingBox.Width, (int)waterArea.BoundingBox.Height), Color.Black);
                }*/

                _spriteBatch.Draw(_texture, body.BoundingBox.Min, new Rectangle((int)body.BoundingBox.Min.X, (int)body.BoundingBox.Min.Y, (int)body.BoundingBox.Width, (int)body.BoundingBox.Height), Color.Black);

            }

            _spriteBatch.End();
        }
    }
}
