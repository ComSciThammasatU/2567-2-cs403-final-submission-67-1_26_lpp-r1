using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinLeafIsle.DayTimeWeather;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using FinLeafIsle.Components.UI;
using static System.Net.Mime.MediaTypeNames;
using MonoGame.Extended.Input;

namespace FinLeafIsle
{
    public class Guide
    {
        public bool Open = false;
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private readonly ViewportAdapter _viewportAdapter;
        private GameState _gameState;
        private AudioManager _audioManager;

        private Button CloseB;
        public Guide(IContainer container)
        {
            _content = container.Resolve<ContentManager>();
            _spriteBatch = container.Resolve<SpriteBatch>();
            _viewportAdapter = container.Resolve<ViewportAdapter>();
            _gameState = container.Resolve<GameState>();
            _audioManager = container.Resolve<AudioManager>();

            CloseB = new Button
            {
                Position = new Vector2(240 + 156, 270 / 2 - 100),
                Size = new Vector2(16, 16),
                Offset = new Vector2(8, 8),
                Texture = _content.Load<Texture2D>($"Guide/CloseB")
            };
        }

        public void Update(GameTime gameTime)
        {
            if (_gameState.State == GState.GamePlay || _gameState.State == GState.Playermenu)
            {
                if (Open)
                {
                    var mouseState = MouseExtended.GetState();
                    var virtualMousePosition = _viewportAdapter.PointToScreen(mouseState.X, mouseState.Y);

                    if (CloseB.BoundingBox.Contain(virtualMousePosition) && mouseState.WasButtonPressed(MouseButton.Left))
                    {
                        _audioManager._pressS.Play();
                        Open = false;
                    }
                }
            }
        }
        public void Draw(GameTime gameTime)
        {

            if (_gameState.State == GState.GamePlay || _gameState.State == GState.Playermenu)
            {
                if (Open)
                {
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _viewportAdapter.GetScaleMatrix());

                    Texture2D guide = _content.Load<Texture2D>("Guide/Guide");
                    _spriteBatch.Draw(guide, new Vector2(240 - 160, 270 / 2 - 104), Color.White);

                    _spriteBatch.Draw(CloseB.Texture, CloseB.Position - CloseB.Offset, Color.White);

                    _spriteBatch.End();
                }
            }

        }
    }
}
