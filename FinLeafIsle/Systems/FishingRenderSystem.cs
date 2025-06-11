using FinLeafIsle.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using Autofac;
using FinLeafIsle.Components;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.ViewportAdapters;
using FinLeafIsle.Components.Inventory;
using System;
using MonoGame.Extended.Particles;
using FinLeafIsle.Components.MiniGame;
using FinLeafIsle.Components.ItemComponent;
using System.Reflection;

namespace FinLeafIsle.Systems
{
    public class FishingRenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;
        private readonly ContentManager _content;
        private GameState _gameState;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<FishingComponent> _fishingMapper;
        private ComponentMapper<FishingMiniGameComponent> _fishingMiniGameMapper;
        private InventorySlot _handSlot;

        private Texture2DAtlas _boberAtlas;
        private Sprite _bober;
        private SpriteSheet _boberSheet;
        private AnimatedSprite _bobbing;

        public FishingRenderSystem(IContainer container)
         : base(Aspect.All(typeof(Player), typeof(FishingComponent)))
        {
            _spriteBatch = container.Resolve<SpriteBatch>();
            _camera = container.Resolve<OrthographicCamera>();
            _gameState = container.Resolve<GameState>();
            _content = container.Resolve<ContentManager>();
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _playerMapper = mapperService.GetMapper<Player>();
            _fishingMapper = mapperService.GetMapper<FishingComponent>();
            _fishingMiniGameMapper = mapperService.GetMapper<FishingMiniGameComponent>();

            Texture2D boberTexture = _content.Load<Texture2D>("Items/FishingItems/Bober");
            _boberAtlas = Texture2DAtlas.Create("Atlas/bober", boberTexture, 16, 16);
            _boberSheet = new SpriteSheet("SpriteSheet/bober", _boberAtlas);
            _boberSheet.DefineAnimation("bobbing", builder =>
            {
                builder.IsLooping(true)
                        .AddFrame(regionIndex: 0, duration: TimeSpan.FromSeconds(0.1))
                        .AddFrame(2, TimeSpan.FromSeconds(0.1));
            });
            _bobbing = new AnimatedSprite(_boberSheet, "bobbing");
        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

                foreach (var entity in ActiveEntities)
                {
                    var transform = _transformMapper.Get(entity);
                    var fishing = _fishingMapper.Get(entity);

                    switch (fishing.State)
                    {
                        case FishingState.Charging:
                            if (_handSlot._item.Has<FishingRod>())
                            {
                                FishingRod rod = _handSlot._item.Get<FishingRod>();
                                float min = 32f;
                                float max = rod.MaxCastingDistance;
                                float percent = (fishing.Timer / rod.MaxChargeTime) % 2f; // Loop 0 → 2
                                float pingPong = percent > 1f ? 2f - percent : percent; // PingPong effect between 0 → 1 → 0

                                float castDistance = MathHelper.Lerp(min, max, pingPong);
                                float chargePercent = (castDistance - min) / (max - min); // 0 to 1

                                Vector2 barSize = new Vector2(40, 5);
                                Vector2 barPosition = new Vector2(transform.Position.X - 20, transform.Position.Y - 50); // above player
                                Rectangle backgroundBar = new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)barSize.X, (int)barSize.Y);
                                Rectangle filledBar = new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)(barSize.X * chargePercent), (int)barSize.Y);
                                Texture2D _whitePixel = _content.Load<Texture2D>("pixel");
                                _spriteBatch.Draw(_whitePixel, backgroundBar, Color.DarkGray);

                                Color barColor;
                                if (chargePercent < 0.3f)
                                {
                                    // From Red to Yellow
                                    float t = chargePercent / 0.3f;
                                    barColor = Color.Lerp(Color.OrangeRed, Color.Yellow, t);
                                }
                                else
                                {
                                    // From Yellow to Green
                                    float t = (chargePercent - 0.3f) / 0.3f;
                                    barColor = Color.Lerp(Color.Yellow, Color.GreenYellow, t);
                                }
                                _spriteBatch.Draw(_whitePixel, filledBar, barColor);

                            }
                            break;

                        case FishingState.MovingHook:
                            _bober = _boberAtlas.CreateSprite(regionIndex: 0);
                            _spriteBatch.Draw(_bober, new Vector2(fishing.HookCurrentPosition.X - 3, fishing.HookCurrentPosition.Y - 3), 0f);
                            break;
                        case FishingState.WaitingForFish:
                            if (_bobbing is AnimatedSprite animatedSprite)
                            {
                                animatedSprite.Update(gameTime);
                            }
                            _spriteBatch.Draw(_bobbing, new Vector2(fishing.HookCurrentPosition.X - 3, fishing.HookCurrentPosition.Y - 3), 0f);
                            break;

                        case FishingState.FishOnHook:
                            _bober = _boberAtlas.CreateSprite(regionIndex: 2);
                            _spriteBatch.Draw(_bober, new Vector2(fishing.HookCurrentPosition.X - 3, fishing.HookCurrentPosition.Y - 3), 0f);
                            break;

                        case FishingState.Minigame:

                            if (_fishingMiniGameMapper.Has(entity))
                            {
                                var fishingMiniGame = _fishingMiniGameMapper.Get(entity);

                                // Draw big circle
                                Texture2D bigCircleTexture = _content.Load<Texture2D>("Player/Fishing/bigCircle");
                                _spriteBatch.Draw(
                                    bigCircleTexture,
                                    fishingMiniGame.Center,
                                    null,
                                    Color.White * 1f,
                                    0f,
                                    new Vector2(bigCircleTexture.Width / 2f, bigCircleTexture.Height / 2f),
                                    1f,
                                    SpriteEffects.None,
                                    0f
                                );
                                // Draw small circle
                                Texture2D smallCircleTexture = _content.Load<Texture2D>("Player/Fishing/smallCircle");
                                _spriteBatch.Draw(
                                    smallCircleTexture,
                                    fishingMiniGame.Center,
                                    null,
                                    Color.White * 1f,
                                    0f,
                                    new Vector2(smallCircleTexture.Width / 2f, smallCircleTexture.Height / 2f),
                                    1f,
                                    SpriteEffects.None,
                                    0f
                                );
                                //Draw Filling circle
                                var fish = fishing.FishOnHook.Get<Fish>();
                                float percent = MathHelper.Clamp(fishingMiniGame.Progress / fish.Difficult, 0f, 1f);

                                Texture2D smallCircleFilledTexture = _content.Load<Texture2D>("Player/Fishing/smallCircleFilled");
                                int fullHeight = smallCircleFilledTexture.Height;
                                int fillHeight = (int)MathF.Ceiling(fullHeight * percent);
                                int startY = fullHeight - fillHeight;

                                Rectangle sourceRect = new Rectangle(0, startY, smallCircleFilledTexture.Width, fillHeight);
                                Vector2 origin = new Vector2(smallCircleFilledTexture.Width / 2f, 0f);
                                Vector2 drawPosition = new Vector2(fishingMiniGame.Center.X, fishingMiniGame.Center.Y + startY - fullHeight / 2f);
                                _spriteBatch.Draw(
                                    smallCircleFilledTexture,
                                    drawPosition, // shifted up by clipped amount
                                    sourceRect,
                                    Color.GreenYellow * 1f, // or change color as you wish
                                    0f,
                                    origin,
                                    1f,
                                    SpriteEffects.None,
                                    0f
                                );
                                
                            }
                            _bober = _boberAtlas.CreateSprite(regionIndex: 2);
                            _spriteBatch.Draw(_bober, new Vector2(fishing.HookCurrentPosition.X - 3, fishing.HookCurrentPosition.Y - 3), 0f);
                            break;
                    }
                }
                _spriteBatch.End();

            }
        }
    }
}
