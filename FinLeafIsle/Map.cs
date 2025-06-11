using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Autofac;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Net.Mime;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Collisions.Layers;
using System;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Collisions;

namespace FinLeafIsle
{
    public class Map
    {
        private TiledMap _map;
        private TiledMapRenderer _renderer;

        public Map() { }
        
        public void LoadMap(ContentManager contentManager, GraphicsDevice _graphicsDevice, MapLocation _currentLocation, GameWorld world)
        {
            var name = _currentLocation.Location.ToString();
            System.Diagnostics.Debug.WriteLine($"Maps/{name}");
            _map = contentManager.Load<TiledMap>($"Maps/{name}");
            _renderer = new TiledMapRenderer(_graphicsDevice, _map);

            foreach (var layer in _map.ObjectLayers)
            {
                
                if (layer.Name == "Walls")
                {
                    foreach (var obj in layer.Objects)
                    {
                        
                        if (obj.Type == "Wall")
                        {
                            //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {obj.Size}");
                            Vector2 position = new Vector2(obj.Position.X + obj.Size.Width * 0.5f, obj.Position.Y + obj.Size.Height * 0.5f);
                            GameMain._entityFactory.CreateWall(position, obj.Size);
                        }
                    }
                }
                if (layer.Name == "WaterAreas")
                {
                    foreach (var obj in layer.Objects)
                    {

                        if (obj.Type == "WaterArea")
                        {
                            //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {obj.Size}");
                            Vector2 position = new Vector2(obj.Position.X + obj.Size.Width * 0.5f, obj.Position.Y + obj.Size.Height * 0.5f);
                            int depth = 0;
                            if (obj.Properties.TryGetValue("Depth", out String depthString))
                            {
                                int.TryParse(depthString, out depth); 
                            }
                            int location = 0;
                            if (obj.Properties.TryGetValue("Location", out String locationString))
                            {
                                int.TryParse(locationString, out location);
                            }
                            GameMain._entityFactory.CreateWaterArea(position, obj.Size, depth);
                        }
                    }
                }
                if (layer.Name == "GateAreas")
                {
                    foreach (var obj in layer.Objects)
                    {

                        if (obj.Type == "Gate")
                        {
                            //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {obj.Size}");
                            Vector2 position = new Vector2(obj.Position.X + obj.Size.Width * 0.5f, obj.Position.Y + obj.Size.Height * 0.5f);
                            string destination = "";
                            if (obj.Properties.TryGetValue("Destination", out String location))
                            {
                                destination = location;
                            }
                            Vector2 target = new Vector2();
                            if (obj.Properties.TryGetValue("TargetX", out String tx) && obj.Properties.TryGetValue("TargetY", out String ty))
                            {
                                int x = 0;
                                int y = 0;
                                int.TryParse(tx, out x);
                                int.TryParse(ty, out y);
                                target = new Vector2(x, y);
                            }
                            world._gateAreas.Add(new GateArea(position, obj.Size, destination, target));
                        }
                    }
                }
                if (layer.Name == "CameraBlocks")
                {
                    foreach (var obj in layer.Objects)
                    {

                        if (obj.Type == "CameraBlock")
                        {
                            //System.Diagnostics.Debug.WriteLine($"Found Ya!!! {obj.Size}");
                            Vector2 position = new Vector2(obj.Position.X + obj.Size.Width * 0.5f, obj.Position.Y + obj.Size.Height * 0.5f);
                            
                            world._cameraBlocks.Add(new CameraBlock(position, obj.Size));
                        }
                    }
                }
            }
            
        }

        public void Update(GameTime gameTime)
        {
            _renderer.Update(gameTime);
        }

        public void Draw(GameTime gameTime, ContentManager contentManager, SpriteBatch _spriteBatch, OrthographicCamera _camera)
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

            _renderer.Draw(_camera.GetViewMatrix());


            foreach (var layer in _map.ObjectLayers)
            {
                if (layer.Name == "Decorations") { 
                    foreach (var obj in layer.Objects)
                    {
                        if (obj.Type == "Decoration")
                        {
                            if (obj.Properties.TryGetValue("ObjName", out string objName)) // ✅ Get actual name
                            {
                                Texture2D objectTexture = contentManager.Load<Texture2D>($"Map/Decorations/{objName}"); // ✅ Use objName
                                Vector2 position = new Vector2(obj.Position.X, obj.Position.Y - obj.Size.Height);

                                _spriteBatch.Draw(objectTexture, position, Color.White);
                            }
                            else
                            {
                                Console.WriteLine($"Object {obj.Identifier} has no 'ObjName' property.");
                            }
                        }
                    }
                }
            }
            /*
            foreach (var layer in _map.ObjectLayers)
            {

                if (layer.Name == "Walls")
                {
                    foreach (var obj in layer.Objects)
                    {

                        if (obj.Type == "Wall")
                        {
                            var position = new Vector2(obj.Position.X, obj.Position.Y);
                            Texture2D _texture = _contentManager.Load<Texture2D>("pixel");
                            _spriteBatch.Draw(_texture, position,new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.Width, (int)obj.Size.Height), Color.Black);
                        }
                    }
                }
            }*/
            _spriteBatch.End();
        }

    }
}
