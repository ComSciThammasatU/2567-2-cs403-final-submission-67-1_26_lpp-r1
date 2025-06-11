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
using FinLeafIsle.DayTimeWeather;

namespace FinLeafIsle
{
    public class PlayerMenu
    {
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly ViewportAdapter _viewportAdapter;
        private GameState _gameState;
        private SaveManager _saveManager;
        private MapState _mapState;
        private AudioManager _audioManager;
        private SoundtrackManager _soundtrackManager;

        private Button InventoryB;
        private Button SettingB;
        private Button SFXInc;
        private Button SFXDec;
        private Button MusicInc;
        private Button MusicDec;
        private Button MenuB;
        private Button MainMenuB;
        private Button ExitB;
        private Button HelpB;

        public static SpriteFont Font;
        private Guide _guide;
        public PlayerMenu(IContainer container, Guide guide)
        {
            _content = container.Resolve<ContentManager>();
            _spriteBatch = container.Resolve<SpriteBatch>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
            _gameState = container.Resolve<GameState>();
            _saveManager = container.Resolve<SaveManager>();
            _mapState = container.Resolve<MapState>();
            _audioManager = container.Resolve<AudioManager>();
            _soundtrackManager = container.Resolve<SoundtrackManager>();
            _guide = guide;

            Font = _content.Load<SpriteFont>("DebugFont");

            _gameState.PMenu = PMenuState.Inventory;
            InventoryB = new Button
            {
                Position = new Vector2(135, 65),
                Size = new Vector2(48, 16),
                Offset = new Vector2(24, 8),
                Texture = _content.Load<Texture2D>($"PlayerMenu/InventoryB")
            };
            //Setting Page
            SettingB = new Button
            {
                Position = new Vector2(183, 65),
                Size = new Vector2(48, 16),
                Offset = new Vector2(24, 8),
                Texture = _content.Load<Texture2D>($"PlayerMenu/SettingB")
            };
            SFXInc = new Button
            {
                Position = new Vector2(320, 115),
                Size = new Vector2(32, 32),
                Offset = new Vector2(16, 16),
                Texture = _content.Load<Texture2D>($"PlayerMenu/IncreaseB")
            };
            SFXDec = new Button
            {
                Position = new Vector2(240, 115),
                Size = new Vector2(32, 32),
                Offset = new Vector2(16, 16),
                Texture = _content.Load<Texture2D>($"PlayerMenu/DecreaseB")
            };
            MusicInc = new Button
            {
                Position = new Vector2(320, 155),
                Size = new Vector2(32, 32),
                Offset = new Vector2(16, 16),
                Texture = _content.Load<Texture2D>($"PlayerMenu/IncreaseB")
            };
            MusicDec = new Button
            {
                Position = new Vector2(240, 155),
                Size = new Vector2(32, 32),
                Offset = new Vector2(16, 16),
                Texture = _content.Load<Texture2D>($"PlayerMenu/DecreaseB")
            };

            //Menu Page
            MenuB = new Button
            {
                Position = new Vector2(183+48, 65),
                Size = new Vector2(48, 16),
                Offset = new Vector2(24, 8),
                Texture = _content.Load<Texture2D>($"PlayerMenu/MenuB")
            };
            MainMenuB = new Button
            {
                Position = new Vector2(240, 115),
                Size = new Vector2(100, 30),
                Offset = new Vector2(50, 15),
                Texture = _content.Load<Texture2D>($"PlayerMenu/MainB")
            };
            ExitB = new Button
            {
                Position = new Vector2(240, 155),
                Size = new Vector2(100, 30),
                Offset = new Vector2(50, 15),
                Texture = _content.Load<Texture2D>($"PlayerMenu/ExitB")
            };

            HelpB = new Button
            {
                Position = new Vector2(360, 65),
                Size = new Vector2(16, 16),
                Offset = new Vector2(8, 8),
                Texture = _content.Load<Texture2D>($"PlayerMenu/HelpB")
            };

        }
        public void Update(GameTime gameTime)
        {
            var mouseState = MouseExtended.GetState();
            var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

            if (InventoryB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
            {
                _audioManager._pressS.Play();
                _gameState.PMenu = PMenuState.Inventory;
            }
            if (SettingB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
            {
                _audioManager._pressS.Play();
                _gameState.PMenu = PMenuState.Setting;
            }
            if (MenuB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
            {
                _audioManager._pressS.Play();
                _gameState.PMenu = PMenuState.Menu;

            }
            if (HelpB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
            {
                _audioManager._pressS.Play();
                _guide.Open = true;

            }

            switch (_gameState.PMenu)
            {
                case PMenuState.Setting:

                    if (SFXInc.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _audioManager.IncVolume();
                        
                    }
                    if (SFXDec.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _audioManager.DecVolume();

                    }
                    if (MusicInc.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _soundtrackManager.IncVolume();

                    }
                    if (MusicDec.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _soundtrackManager.DecVolume();
                    }


                    break;
                case PMenuState.Menu:

                    if (MainMenuB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        _mapState._state = MapLoaderState.Unload;
                        _saveManager.ClearTemp();
                        _gameState.State = GState.MainMenu;
                    }

                    
                    break;  
            }
        }
        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());

            Texture2D objectTexture = _content.Load<Texture2D>("pixel");
            //Overlay
            _spriteBatch.Draw(objectTexture,
                    new Vector2(0, 0),
                    new Rectangle(0, 0, 480, 270),
                    Color.Black * 0.5f,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    1f);
            //BgMenu
            _spriteBatch.Draw(objectTexture,
                    new Vector2(110, 70),
                    new Rectangle(0, 0, 260, 130),
                    Color.SandyBrown,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    1f);

            _spriteBatch.Draw(InventoryB.Texture, InventoryB.Position - InventoryB.Offset, Color.White);
            _spriteBatch.Draw(SettingB.Texture, SettingB.Position - SettingB.Offset, Color.White);
            _spriteBatch.Draw(MenuB.Texture, MenuB.Position - MenuB.Offset, Color.White);
            _spriteBatch.Draw(HelpB.Texture, HelpB.Position - HelpB.Offset, Color.White);
            switch (_gameState.PMenu)
            {
                case PMenuState.Setting:
                    Texture2D SFXLogo = _content.Load<Texture2D>("PlayerMenu/SFXLogo");
                    _spriteBatch.Draw(SFXLogo, new Vector2(SFXDec.Position.X - 90, SFXDec.Position.Y-16), Color.White);
                    _spriteBatch.Draw(SFXInc.Texture, SFXInc.Position - SFXInc.Offset, Color.White);
                    _spriteBatch.DrawString(Font, $" {_audioManager.GetVolume():F1}", new Vector2(SFXDec.Position.X+24, SFXDec.Position.Y - 16), Color.White);
                    _spriteBatch.Draw(SFXDec.Texture, SFXDec.Position - SFXDec.Offset, Color.White);
                    Texture2D MusicLogo = _content.Load<Texture2D>("PlayerMenu/MusicLogo");
                    _spriteBatch.Draw(MusicLogo, new Vector2(MusicDec.Position.X - 90, MusicDec.Position.Y-16), Color.White);
                    _spriteBatch.Draw(MusicInc.Texture, MusicInc.Position - MusicInc.Offset, Color.White);
                    _spriteBatch.DrawString(Font, $" {_soundtrackManager.GetVolume():F1}", new Vector2(MusicDec.Position.X + 24, MusicDec.Position.Y - 16), Color.White);
                    _spriteBatch.Draw(MusicDec.Texture, MusicDec.Position - MusicDec.Offset, Color.White);
                    break;
                case PMenuState.Menu:

                    _spriteBatch.Draw(MainMenuB.Texture, MainMenuB.Position - MainMenuB.Offset, Color.White);
                    _spriteBatch.Draw(ExitB.Texture, ExitB.Position - ExitB.Offset, Color.White);
                    break;
            }

            _spriteBatch.End();
        }
        public bool Exit()
        {
            if (_gameState.PMenu == PMenuState.Menu)
            {
                var mouseState = MouseExtended.GetState();
                var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

                if (ExitB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    _audioManager._pressS.Play();
                    _mapState._state = MapLoaderState.Unload;
                    _saveManager.ClearTemp();
                    return true;
                }
            }
            return false;
        }
    }
}
