using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Autofac;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Input;
using FinLeafIsle.Systems;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.DayTimeWeather;
using FinLeafIsle.Collisions;
using Autofac.Core.Lifetime;
using System;
using System.Reflection;
using System.IO;
using System.Xml.Linq;



//System.Diagnostics.Debug.WriteLine($"Found Ya!!!  {inventorySlot.BoundingBox.Width}");
//Auto Format: Ctrl + K, Ctrl + D

namespace FinLeafIsle
{
    public class GameMain : Game1
    {
        private GameState _gameState;
        private OrthographicCamera _camera;
        private World _world;
        private GameWorld _gameWorld;
        private MapState _mapState;
        private ViewportAdapter _viewportAdapter;
        private SpriteBatch _spriteBatch;
        private InventorySlot _mouseInventorySlot;
        private InventorySlot _handSlot;
        private MapLocation _currentLocation;
        private MapLocation _nextLocation;
        private MapLocation _bedLocation;
        private DayTime _dayTime;
        private Map _map;
        private MainMenu _mainMenu;
        private SaveManager _saveManager;
        private PlayerMenu _playerMenu;
        private SaveDayPage _saveDayPage;
        private Guide _guide;

        private AudioManager _audioManager;
        private SoundtrackManager _soundtrackManager;

        public static EntityFactory _entityFactory;
        public static SpriteFont DebugFont;

        private double _inputCooldown = 0;

        public GameMain() { }

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 480, 270);
            _camera = new OrthographicCamera(_viewportAdapter);
            _gameState = new GameState { State = GState.MainMenu };
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mouseInventorySlot = new InventorySlot { isHovered = false, isPressed = false, _item = null };
            _handSlot = new InventorySlot { Position = new Vector2(190, 80) + (new Vector2(16, 16) / 2), Size = new Vector2(16, 16), isHovered = false, isPressed = false, _item = null };
            _gameWorld = new GameWorld(new Vector2(0, 0));
            _currentLocation = new MapLocation();
            _nextLocation = new MapLocation();
            _bedLocation = new MapLocation();
            _dayTime = new DayTime();
            _map = new Map();
            _mapState = new MapState();
            _saveManager = new SaveManager(_dayTime);
            _audioManager = new AudioManager(Content);
            _soundtrackManager = new SoundtrackManager(Content);


            builder.RegisterInstance(_viewportAdapter);
            builder.RegisterInstance(Content);
            builder.RegisterInstance(GraphicsDevice);
            builder.RegisterInstance(_spriteBatch);
            builder.RegisterInstance(_camera);
            builder.RegisterInstance(_gameState);
            builder.RegisterInstance(_mouseInventorySlot).Named<InventorySlot>("MouseSlot");
            builder.RegisterInstance(_handSlot).Named<InventorySlot>("HandSlot");
            builder.RegisterInstance(_gameWorld);
            builder.RegisterInstance(_currentLocation).Named<MapLocation>("CurrentLocation");
            builder.RegisterInstance(_nextLocation).Named<MapLocation>("NextLocation");
            builder.RegisterInstance(_bedLocation).Named<MapLocation>("BedLocation");
            builder.RegisterInstance(new Random());
            builder.RegisterInstance(_dayTime);
            builder.RegisterInstance(_map);
            builder.RegisterInstance(_mapState);
            builder.RegisterInstance(_saveManager);
            builder.RegisterInstance(_audioManager);
            builder.RegisterInstance(_soundtrackManager);
        }
        protected override void LoadContent()
        {
            
            _saveDayPage = new SaveDayPage(Container);
            _guide = new Guide(Container);
            _playerMenu = new PlayerMenu(Container, _guide);

            _world = new WorldBuilder()
                .AddSystem(new WorldSystem(Container))
                .AddSystem(new PlayerSystem(Container))
                .AddSystem(new GateSystem(Container, _saveDayPage))
                .AddSystem(new MapLoaderSystem(Container))

                //.AddSystem(new BodyRenderSystem(new SpriteBatch(GraphicsDevice), _camera, Content))
                .AddSystem(new RenderSystem(Container))
                .AddSystem(new FishingRenderSystem(Container))
                .AddSystem(new LightingRenderSystem(Container))
                .AddSystem(new HUDRenderSystem(Container))
                .AddSystem(new PlayerMenuSystem(Container))
                .AddSystem(new PlayerMenuRenderSystem(Container, _guide))
                .AddSystem(new MapRenderSystem(Container))
                .AddSystem(new WaterAreaSystem(Container))
                .AddSystem(new FishingSystem(Container))
                .AddSystem(new FishingMiniGameSystem(Container))
                .AddSystem(new CameraSystem(Container))
                .Build();

            Components.Add(_world);

            _entityFactory = new EntityFactory(_world, Content, _gameWorld);
            
            _mainMenu = new MainMenu(Container, _guide);
            
            FishingRod WoodRod = new FishingRod(92, 1.1f);
            Item rodItem = new Item(0, "WoodRod", "Rod from wood.");

            _handSlot._item = _entityFactory.CreateFishingRod(rodItem, WoodRod);
            DebugFont = Content.Load<SpriteFont>("DebugFont");

            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            _soundtrackManager._m1.IsLooped = true;
            _soundtrackManager._m1.Play();

            KeyboardExtended.Update();
            MouseExtended.Update();

            var keyboardState = KeyboardExtended.GetState();

            

            //GameStateManage:
            _inputCooldown -= gameTime.ElapsedGameTime.TotalSeconds;
            if (_gameState.State == GState.MainMenu)
            {
                _mainMenu.Update(gameTime);

                if (_inputCooldown <= 0)
                {
                    if (keyboardState.WasKeyPressed(Keys.Enter)) // Play
                    {
                        _gameState.State = GState.GamePlay;
                        _entityFactory.CreatePlayer(new Vector2(100, 240), true);
                        _inputCooldown = 0.2;
                    }
                    if (_mainMenu.Exit())
                    {
                        Exit();
                    }
                    
                }
            }

            if (_gameState.State == GState.GamePlay)
            {
                if (_inputCooldown <= 0)
                {
                    if (keyboardState.WasKeyPressed(Keys.E))
                    {
                        _audioManager.PressSound.Play();
                        _gameState.State = GState.Playermenu;
                        _inputCooldown = 0.2;
                    }
                }
                _dayTime.Update(gameTime);
                DebugOverlay.AddLine($"[ Day / Time ]");
                DebugOverlay.AddLine($"{_dayTime.Day} / {_dayTime.Time}");

            }

            if (_gameState.State == GState.Playermenu)
            {
                if (_inputCooldown <= 0)
                {
                    _playerMenu.Update(gameTime);

                    if (keyboardState.WasKeyPressed(Keys.E))
                    {
                        _audioManager.PressSound.Play();
                        _gameState.State = GState.GamePlay;
                        _inputCooldown = 0.2;
                    }

                    if (_playerMenu.Exit())
                    {
                        Exit();
                    }
                }
            }

            if (keyboardState.WasKeyPressed(Keys.T))
            {
                if (_dayTime.Time < 1200)
                    _dayTime.Time = 1200; // Noon
                else if (_dayTime.Time < 1700)
                    _dayTime.Time = 1700; // Evening
                else if (_dayTime.Time < 2100)
                    _dayTime.Time = 2100; // Night
                else if (_dayTime.Time < 2550)
                    _dayTime.Time = 2550; // Night
                else
                    _dayTime.Time = 600;  // Back to morning
            }


            _saveDayPage.Update(gameTime);
            _guide.Update(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            if (_gameState.State == GState.MainMenu)
            {
                _mainMenu.Draw(gameTime);
            }
            if (_gameState.State == GState.GamePlay || _gameState.State == GState.Playermenu)
            {
                if (_mapState._state == MapLoaderState.Idle)
                    _map.Draw(gameTime, Content, _spriteBatch, _camera);
            }
            if (_gameState.State == GState.Playermenu)
            {
                _playerMenu.Draw(gameTime);
            }

            _spriteBatch.Begin();

            DebugOverlay.Draw(_spriteBatch, DebugFont);
            _spriteBatch.End();

            _saveDayPage.Draw(gameTime);
            //_guide.Draw(gameTime);
            base.Draw(gameTime);

        }
        

    }
}