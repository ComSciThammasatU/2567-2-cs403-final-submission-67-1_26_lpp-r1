using Autofac;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using System.Xml.Linq;


namespace FinLeafIsle.Systems
{
    public class PlayerMenuSystem : EntityProcessingSystem
    {
        private readonly ViewportAdapter _viewportAdapter;
        private GameState _gameState;
        private InventorySlot _mouseInventorySlot;
        private InventorySlot _handSlot;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<InventoryComponent> _inventoryMapper;
        private readonly OrthographicCamera _camera;
        private AudioManager _audioManager;


        public PlayerMenuSystem(IContainer container)
            : base(Aspect.All(typeof(Body), typeof(Player), typeof(Transform2), typeof(InventoryComponent)))
        {

            _gameState = container.Resolve<GameState>();
            _mouseInventorySlot = container.ResolveNamed<InventorySlot>("MouseSlot");
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
            _camera = container.Resolve<OrthographicCamera>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
            _audioManager = container.Resolve<AudioManager>();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _playerMapper = mapperService.GetMapper<Player>();
            _inventoryMapper = mapperService.GetMapper<InventoryComponent>();

        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_gameState.State == GState.Playermenu)
            {
                if (_gameState.PMenu == PMenuState.Inventory)
                {
                    var transform = _transformMapper.Get(entityId);
                    var body = _bodyMapper.Get(entityId);
                    var inventory = _inventoryMapper.Get(entityId);

                    var keyboardState = KeyboardExtended.GetState();
                    var mouseState = MouseExtended.GetState();

                    var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);


                    //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {inventory._inventorySlot[i].Position} {i}");


                   
                    foreach (var inventorySlot in inventory._inventorySlot)
                    {
                        if (inventorySlot.BoundingBox.Contain(virtualMousePosition))
                        {
                            inventorySlot.isHovered = true;

                            if (mouseState.WasButtonPressed(MouseButton.Left))
                            {
                                _audioManager.PressSound.Play();
                                //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {inventorySlot.Position} ");
                                if (inventorySlot._item != null && _mouseInventorySlot._item == null)
                                {
                                    _mouseInventorySlot._item = inventorySlot._item;
                                    inventorySlot._item = null;
                                }
                                else if (inventorySlot._item == null && _mouseInventorySlot._item != null)
                                {
                                    inventorySlot._item = _mouseInventorySlot._item;
                                    _mouseInventorySlot._item = null;
                                }
                                else if (inventorySlot._item != null && _mouseInventorySlot._item != null)
                                {
                                    var temp = inventorySlot._item;
                                    inventorySlot._item = _mouseInventorySlot._item;
                                    _mouseInventorySlot._item = temp;
                                }
                            }
                        }
                        else
                        {
                            inventorySlot.isHovered = false;
                        }


                    }

                    if (_handSlot.BoundingBox.Contain(virtualMousePosition))
                    {
                        _handSlot.isHovered = true;
                        if (mouseState.WasButtonPressed(MouseButton.Left))
                        {
                            _audioManager.PressSound.Play();
                            //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {inventorySlot.Position} ");
                            if (_handSlot._item != null && _mouseInventorySlot._item == null)
                            {
                                _mouseInventorySlot._item = _handSlot._item;
                                _handSlot._item = null;
                            }
                            else if (_handSlot._item == null && _mouseInventorySlot._item != null)
                            {
                                _handSlot._item = _mouseInventorySlot._item;
                                _mouseInventorySlot._item = null;
                            }
                            else if (_handSlot._item != null && _mouseInventorySlot._item != null)
                            {
                                var temp = _handSlot._item;
                                _handSlot._item = _mouseInventorySlot._item;
                                _mouseInventorySlot._item = temp;
                            }
                        }
                    }
                    else
                    {
                        _handSlot.isHovered = false;
                    }
                }

            }
        }
    }
}

