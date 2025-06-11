using FinLeafIsle.Components.UI;
using Microsoft.Xna.Framework;
using Autofac;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Input;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using MonoGame.Extended.ECS.Systems;
using FinLeafIsle.Components;

namespace FinLeafIsle
{
    public enum MenuPage { Main, Save}
    public class MainMenu
    {
        private readonly ContentManager _contentManager;
        private readonly SpriteBatch _spriteBatch;
        private readonly ViewportAdapter _viewportAdapter;
        private MapState _mapState;
        private GameState _gameState;
        private SaveManager _saveManager;
        private MapLocation _nextMap; 
        private MapLocation _bedLocation;
        private AudioManager _audioManager;
        private MenuPage _menuPage;
        private Guide _guide;

        //Main Page
        private Button PlayB;
        private Button ExitB;

        //Save Page
        private Button Save1;
        private Button Save2;
        private Button Save3;
        private Button BackB;
        public MainMenu(IContainer container, Guide guide) 
        {
            _mapState = container.Resolve<MapState>(); ;
            _contentManager = container.Resolve<ContentManager>();
            _spriteBatch = container.Resolve<SpriteBatch>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
            _gameState = container.Resolve<GameState>();
            _saveManager = container.Resolve<SaveManager>();
            _nextMap = container.ResolveNamed<MapLocation>("NextLocation");
            _bedLocation = container.ResolveNamed<MapLocation>("BedLocation");
            _audioManager = container.Resolve<AudioManager>();
            _menuPage = MenuPage.Main;
            _guide = guide;
            PlayB = new Button
            {
                Position = new Vector2(240, 130),
                Size = new Vector2(100, 30),
                Offset = new Vector2(50,15),
                Texture = _contentManager.Load<Texture2D>($"MainMenu/PlayB")
            };
            ExitB = new Button
            {
                Position = new Vector2(240, 175),
                Size = new Vector2(100, 30),
                Offset = new Vector2(50, 15),
                Texture = _contentManager.Load<Texture2D>($"MainMenu/ExitB")
            };

            Save1 = new Button
            {
                Position = new Vector2(240, 50),
                Size = new Vector2(206, 46),
                Offset = new Vector2(103, 23),
                Texture = _contentManager.Load<Texture2D>($"MainMenu/Save1")
            };
            Save2 = new Button
            {
                Position = new Vector2(240, 106),
                Size = new Vector2(206, 46),
                Offset = new Vector2(103, 23),
                Texture = _contentManager.Load<Texture2D>($"MainMenu/Save2")
            };
            Save3 = new Button
            {
                Position = new Vector2(240, 162),
                Size = new Vector2(206, 46),
                Offset = new Vector2(103, 23),
                Texture = _contentManager.Load<Texture2D>($"MainMenu/Save3")
            };
            BackB = new Button
            {
                Position = new Vector2(240, 218),
                Size = new Vector2(32, 32),
                Offset = new Vector2(16, 16),
                Texture = _contentManager.Load<Texture2D>($"MainMenu/BackB")
            };
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = MouseExtended.GetState();
            var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);
            
            switch (_menuPage)
            {
                case MenuPage.Main:

                    if (PlayB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _menuPage = MenuPage.Save;
                    }

                    _nextMap.Location = Location.Tent;
                    _nextMap.Target = new Vector2(234, 151);
                    break;
                case MenuPage.Save:
                    string saveRoot = Path.Combine(Environment.CurrentDirectory, "Save");

                    if (BackB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _menuPage = MenuPage.Main;
                    }

                    if (Save1.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _guide.Open = true;
                        string slot1Path = Path.Combine(saveRoot, "slot1");
                        _saveManager._savePath = slot1Path;

                        if (DirectoryHasContent(slot1Path))
                        {
                            _saveManager.CopySaveSlotToTemp();
                        }
                        else
                        {
                            Vector2 bedPosition = new Vector2(304, 168);
                            GameMain._entityFactory.CreateBed(bedPosition, new Vector2(32, 48));
                            _bedLocation.Location = _nextMap.Location;
                            _bedLocation.Target = bedPosition - new Vector2(32, 0);
                        }
                        // Create Save folder if it doesn't exist
                        if (!Directory.Exists(saveRoot))
                        {
                            Directory.CreateDirectory(saveRoot);
                        }
                        // Create slot1 folder inside Save if it doesn't exist
                        if (!Directory.Exists(slot1Path))
                        {
                            Directory.CreateDirectory(slot1Path);
                            
                        }
                        
                        
                        _mapState._state = MapLoaderState.LoadMap;
                        _gameState.State = GState.GamePlay;
                        _menuPage = MenuPage.Main;

                    }
                    if (Save2.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _guide.Open = true;
                        string slot2Path = Path.Combine(saveRoot, "slot2");
                        _saveManager._savePath = slot2Path;

                        if (DirectoryHasContent(slot2Path))
                        {
                            _saveManager.CopySaveSlotToTemp();
                        }
                        else
                        {
                            GameMain._entityFactory.CreateBed(new Vector2(304, 168), new Vector2(32, 48));
                        }

                        if (DirectoryHasContent(slot2Path))
                        {
                            _saveManager.CopySaveSlotToTemp();
                        }
                        // Create Save folder if it doesn't exist
                        if (!Directory.Exists(saveRoot))
                        {
                            Directory.CreateDirectory(saveRoot);
                        }
                        // Create slot1 folder inside Save if it doesn't exist
                        if (!Directory.Exists(slot2Path))
                        {
                            Directory.CreateDirectory(slot2Path);

                        }
                       
                        _mapState._state = MapLoaderState.LoadMap;
                        _gameState.State = GState.GamePlay;
                        _menuPage = MenuPage.Main;


                    }
                    if (Save3.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _guide.Open = true;
                        string slot3Path = Path.Combine(saveRoot, "slot3");
                        _saveManager._savePath = slot3Path;

                        if (DirectoryHasContent(slot3Path))
                        {
                            _saveManager.CopySaveSlotToTemp();
                        }
                        else
                        {
                            GameMain._entityFactory.CreateBed(new Vector2(304, 168), new Vector2(32, 48));
                        }

                        if (DirectoryHasContent(slot3Path))
                        {
                            _saveManager.CopySaveSlotToTemp();
                        }
                        // Create Save folder if it doesn't exist
                        if (!Directory.Exists(saveRoot))
                        {
                            Directory.CreateDirectory(saveRoot);
                        }
                        // Create slot1 folder inside Save if it doesn't exist
                        if (!Directory.Exists(slot3Path))
                        {
                            Directory.CreateDirectory(slot3Path);

                        }

                        _mapState._state = MapLoaderState.LoadMap;
                        _gameState.State = GState.GamePlay;
                        _menuPage = MenuPage.Main;

                    }
                    break;
            }
            

        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());

            var bg = _contentManager.Load<Texture2D>($"MainMenu/MainMenuBG");
            _spriteBatch.Draw(bg, Vector2.Zero, Color.White);

            switch (_menuPage)
            {
                case MenuPage.Main:

                    var logo = _contentManager.Load<Texture2D>($"MainMenu/Logo");
                    _spriteBatch.Draw(logo, new Vector2(240 - 105, 20), Color.White);

                    _spriteBatch.Draw(PlayB.Texture, PlayB.Position - PlayB.Offset, Color.White);
                    _spriteBatch.Draw(ExitB.Texture, ExitB.Position - ExitB.Offset, Color.White);

                    break;
                case MenuPage.Save:

                    _spriteBatch.Draw(Save1.Texture, Save1.Position - Save1.Offset, Color.White);
                    _spriteBatch.Draw(Save2.Texture, Save2.Position - Save2.Offset, Color.White);
                    _spriteBatch.Draw(Save3.Texture, Save3.Position - Save3.Offset, Color.White);
                    _spriteBatch.Draw(BackB.Texture, BackB.Position - BackB.Offset, Color.White);
                    break;
            }
            
            _spriteBatch.End();
        }

        public bool Exit()
        {
            if (_menuPage == MenuPage.Main)
            {
                var mouseState = MouseExtended.GetState();
                var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

                if (ExitB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    _audioManager._pressS.Play();
                    return true;
                }
            }
            return false;
        }

        private bool DirectoryHasContent(string path)
        {
            return Directory.Exists(path) &&
                   (Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0);
        }
    }

    
}
