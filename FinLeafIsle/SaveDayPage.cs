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
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;
using FinLeafIsle.DayTimeWeather;
using FinLeafIsle.Components.Inventory;

namespace FinLeafIsle
{
    public class SaveDayPage
    {
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly ViewportAdapter _viewportAdapter;
        private GameState _gameState;
        private SaveManager _saveManager;
        private MapState _mapState;
        private DayTime _dayTime;
        private MapLocation _nextMap;
        private MapLocation _bedLocation;
        private AudioManager _audioManager;

        public static SpriteFont Font;
        private Button NextB;

        public SaveDayPage(IContainer container)
        {
            _content = container.Resolve<ContentManager>();
            _spriteBatch = container.Resolve<SpriteBatch>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
            _gameState = container.Resolve<GameState>();
            _saveManager = container.Resolve<SaveManager>();
            _mapState = container.Resolve<MapState>();
            _dayTime = container.Resolve<DayTime>();
            _nextMap = container.ResolveNamed<MapLocation>("NextLocation");
            _bedLocation = container.ResolveNamed<MapLocation>("BedLocation");
            _audioManager = container.Resolve<AudioManager>();

            Font = _content.Load<SpriteFont>("DebugFont");

            NextB = new Button
            {
                Position = new Vector2(240, 150),
                Size = new Vector2(32, 32),
                Offset = new Vector2(16, 16),
                Texture = _content.Load<Texture2D>($"SaveDay/NextB")
            };
        }
        public void Update(GameTime gameTime)
        {
            
            if (_gameState.State == GState.SaveDay)
            {
                var mouseState = MouseExtended.GetState();
                var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

                if (NextB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                {
                    _audioManager._pressS.Play();
                    _gameState.State = GState.GamePlay;
                    _mapState._state = MapLoaderState.LoadMap;
                    _dayTime.Time = 600;
                }
            }
            
        }

        public void Draw(GameTime gameTime)
        {
            if (_gameState.State == GState.SaveDay)
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

                _spriteBatch.DrawString(Font, $"DAY: { _dayTime.Day.ToString()}", new Vector2(215, 100), Color.White);
                _spriteBatch.Draw(NextB.Texture, NextB.Position - NextB.Offset, Color.White);

                _spriteBatch.End();
            }
        }

        public void Saved()
        {
            _gameState.State = GState.SaveDay;
            _mapState._state = MapLoaderState.SaveAndUnload;
            
        }
    }
}
