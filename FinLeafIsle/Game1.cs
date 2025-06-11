using Microsoft.Xna.Framework;
using Autofac;
using System;


namespace FinLeafIsle
{
    public abstract class Game1 : Game
    {
        protected GraphicsDeviceManager GraphicsDeviceManager { get; }
        protected IContainer Container { get; private set; }

        public int Width { get; }
        public int Height { get; }
       

        public Game1(int width = 1600, int height = 960 )
        {
            Width = width;
            Height = height;
            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height
            };
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var containerBuilder = new ContainerBuilder();

            RegisterDependencies(containerBuilder);
            Container = containerBuilder.Build();
             
            base.Initialize();
        }

       protected abstract void RegisterDependencies(ContainerBuilder builder);
    }
}
