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
using FinLeafIsle.Components.ItemComponent;


namespace FinLeafIsle.Systems
{
    public class PlayerMenuRenderSystem : EntityDrawSystem
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

        private Guide _guide;


        public PlayerMenuRenderSystem(IContainer container, Guide guide)
         : base(Aspect.All(typeof(Player), typeof(InventoryComponent)).One(typeof(AnimatedSprite), typeof(Sprite)))
        {
            _spriteBatch = container.Resolve<SpriteBatch>();
            _camera = container.Resolve<OrthographicCamera>();
            _gameState = container.Resolve<GameState>();
            _content = container.Resolve<ContentManager>();
            _mouseInventorySlot = container.ResolveNamed<InventorySlot>("MouseSlot");
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
            _viewportAdapter = container.Resolve<ViewportAdapter>();

            _guide = guide;

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _inventoryMapper = mapperService.GetMapper<InventoryComponent>();

            Texture2D itemIconTexture = _content.Load<Texture2D>("ItemIcon");
            _itemIconAtlas = Texture2DAtlas.Create("Atlas/ItemIcon", itemIconTexture, 16, 16);

        }

        public override void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.Playermenu)
            {
                
                if (_gameState.PMenu == PMenuState.Inventory)
                {
                    var mouseState = MouseExtended.GetState();
                    var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

                    Texture2D objectTexture = _content.Load<Texture2D>("pixel");


                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());



                    foreach (var entityId in ActiveEntities)
                    {
                        var inventory = _inventoryMapper.Get(entityId);



                        foreach (var inventorySlot in inventory._inventorySlot)
                        {
                            //System.Diagnostics.Debug.WriteLine($"Found Ya!!!  {inventorySlot.BoundingBox.Width}");
                            if (inventorySlot.isHovered)
                            {
                                _spriteBatch.Draw(objectTexture,
                                    inventorySlot.BoundingBox.Min,
                                    new Rectangle((int)inventorySlot.BoundingBox.Min.X,
                                        (int)inventorySlot.BoundingBox.Min.Y,
                                        (int)inventorySlot.BoundingBox.Width,
                                        (int)inventorySlot.BoundingBox.Height),
                                    Color.Black);
                            }
                            else
                            {
                                _spriteBatch.Draw(objectTexture,
                                    inventorySlot.BoundingBox.Min,
                                    new Rectangle((int)inventorySlot.BoundingBox.Min.X,
                                        (int)inventorySlot.BoundingBox.Min.Y,
                                        (int)inventorySlot.BoundingBox.Width,
                                        (int)inventorySlot.BoundingBox.Height),
                                    Color.White);
                            }

                            if (inventorySlot._item != null)
                            {
                                var item = inventorySlot._item.Get<Item>();
                                _itemIcon = _itemIconAtlas.CreateSprite(regionIndex: item.Id);
                                _spriteBatch.Draw(_itemIcon, inventorySlot.BoundingBox.Min, 0f);
                            }
                        }
                    }

                    if (_handSlot.isHovered)
                    {
                        _spriteBatch.Draw(objectTexture,
                            _handSlot.BoundingBox.Min,
                            new Rectangle((int)_handSlot.BoundingBox.Min.X,
                                (int)_handSlot.BoundingBox.Min.Y,
                                (int)_handSlot.BoundingBox.Width,
                                (int)_handSlot.BoundingBox.Height),
                            Color.Black);
                    }
                    else
                    {
                        _spriteBatch.Draw(objectTexture,
                            _handSlot.BoundingBox.Min,
                            new Rectangle((int)_handSlot.BoundingBox.Min.X,
                            (int)_handSlot.BoundingBox.Min.Y,
                            (int)_handSlot.BoundingBox.Width,
                            (int)_handSlot.BoundingBox.Height),
                            Color.White);
                    }
                    if (_handSlot._item != null)
                    {
                        var item = _handSlot._item.Get<Item>();
                        _itemIcon = _itemIconAtlas.CreateSprite(regionIndex: item.Id);
                        _spriteBatch.Draw(_itemIcon, _handSlot.BoundingBox.Min, 0f);
                    }

                    if (_mouseInventorySlot._item != null)
                    {
                        var item = _mouseInventorySlot._item.Get<Item>();
                        _itemIcon = _itemIconAtlas.CreateSprite(regionIndex: item.Id);
                        _spriteBatch.Draw(_itemIcon, new Vector2(virtualMousePosition.X, virtualMousePosition.Y), 0f);
                    }
                    _spriteBatch.End();
                }
                _guide.Draw(gameTime);
            }
            if (_gameState.State == GState.GamePlay)
                _guide.Draw(gameTime);
        }
    }
}
