using FinLeafIsle.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using Autofac;
using FinLeafIsle.Components;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;
using Microsoft.Xna.Framework.Content;
using static System.Formats.Asn1.AsnWriter;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;


namespace FinLeafIsle.Systems
{
    public class HUDRenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly OrthographicCamera _camera;
        private readonly ContentManager _content;
        private readonly ViewportAdapter _viewportAdapter;
        private GameState _gameState;
        private InventorySlot _mouseInventorySlot;
        private InventorySlot _handSlot;
        private ComponentMapper<InventoryComponent> _inventoryMapper;
        private Texture2DAtlas _itemIconAtlas;
        private Sprite _itemIcon;


        public HUDRenderSystem(IContainer container)
         : base(Aspect.All(typeof(Body), typeof(Player), typeof(Transform2), typeof(InventoryComponent)).One(typeof(AnimatedSprite), typeof(Sprite)))
        {
            _spriteBatch = container.Resolve<SpriteBatch>();
            _camera = container.Resolve<OrthographicCamera>();
            _gameState = container.Resolve<GameState>();
            _content = container.Resolve<ContentManager>();
            _mouseInventorySlot = container.ResolveNamed<InventorySlot>("MouseSlot");
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
            _viewportAdapter = container.Resolve<ViewportAdapter>();

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _inventoryMapper = mapperService.GetMapper<InventoryComponent>();

            Texture2D itemIconTexture = _content.Load<Texture2D>("ItemIcon");
            _itemIconAtlas = Texture2DAtlas.Create("Atlas/ItemIcon", itemIconTexture, 16, 16);

        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay)
            {
                var mouseState = MouseExtended.GetState();
                var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

                Texture2D objectTexture = _content.Load<Texture2D>("pixel");


                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());


                if (_handSlot._item != null)
                {
                    var item = _handSlot._item.Get<Item>();
                    _itemIcon = _itemIconAtlas.CreateSprite(regionIndex: item.Id);
                    _spriteBatch.Draw(_itemIcon, new Vector2(480 - 20, 270 - 20), 0f);
                }


                _spriteBatch.End();

            }
        }
    }
}
