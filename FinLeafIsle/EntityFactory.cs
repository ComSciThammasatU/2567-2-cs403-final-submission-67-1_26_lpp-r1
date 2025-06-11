using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using World = MonoGame.Extended.ECS.World;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.Components.ItemComponent;

namespace FinLeafIsle
{
    public class EntityFactory
    {
        private readonly World _world;
        private readonly ContentManager _contentManager;
        private readonly GameWorld _gameWorld;

        public Entity LocalPlayerEntity { get; private set; }

        public EntityFactory(World world, ContentManager contentManager, GameWorld gameWorld)
        { 
            _world = world;
            _contentManager = contentManager;
            _gameWorld = gameWorld;   
        }

        public Entity CreatePlayer(Vector2 position, bool isLocalPlayer = false)
        {
            var dudeTexture = _contentManager.Load<Texture2D>("Player/player");
            var dudeAtlas = Texture2DAtlas.Create("TextureAtlas//player", dudeTexture, 32, 64);

            var entity = _world.CreateEntity();
            var spriteSheet = new SpriteSheet("SpriteSheet//player", dudeAtlas);

            AddAnimationCycle(spriteSheet, "idle", new[] { 4, 5 });
            AddAnimationCycle(spriteSheet, "idleDown", new[] { 0, 1 });
            AddAnimationCycle(spriteSheet, "idleUp", new[] { 8, 9 });
            AddAnimationCycle(spriteSheet, "walk", new[] { 6, 6, 4, 7, 7, 4 });
            AddAnimationCycle(spriteSheet, "walkDown", new[] { 2, 2, 0, 3, 3, 0 });
            AddAnimationCycle(spriteSheet, "walkUp", new[] { 10, 10, 8, 11, 11, 8 });
            AddAnimationCycle(spriteSheet, "cast", new[] { 14, 15 });
            AddAnimationCycle(spriteSheet, "castDown", new[] { 12, 13 });
            AddAnimationCycle(spriteSheet, "castUp", new[] { 16, 17 });
            AddAnimationCycle(spriteSheet, "fishing", new[] { 20, 21 });
            AddAnimationCycle(spriteSheet, "fishingDown", new[] { 18, 19 });
            AddAnimationCycle(spriteSheet, "fishingUp", new[] { 22, 23 });
            entity.Attach(new AnimatedSprite(spriteSheet, "idle"));
            entity.Attach(new Transform2(position, 0, Vector2.One));
            entity.Attach(new Body { Position = position, Size = new Vector2(16, 16), BodyType = BodyType.Dynamic});
            entity.Attach(new Player());
            entity.Attach(new InventoryComponent( 36));
            entity.Attach(new CameraBlock(position, new Vector2(480, 270)));
            entity.Attach(new Offset { offset = new Vector2(16, 8) });
            entity.Attach(new OnMap());
            if (isLocalPlayer)
                LocalPlayerEntity = entity;

            return entity;
        }
        public Entity CreatePlayer(Transform2 transform, Body body, Player player, InventoryComponent inventory, bool isLocalPlayer = false)
        {
            var dudeTexture = _contentManager.Load<Texture2D>("Player/player");
            var dudeAtlas = Texture2DAtlas.Create("TextureAtlas//player", dudeTexture, 32, 64);

            var entity = _world.CreateEntity();
            var spriteSheet = new SpriteSheet("SpriteSheet//player", dudeAtlas);

            AddAnimationCycle(spriteSheet, "idle", new[] { 4, 5 });
            AddAnimationCycle(spriteSheet, "idleDown", new[] { 0, 1 });
            AddAnimationCycle(spriteSheet, "idleUp", new[] { 8, 9 });
            AddAnimationCycle(spriteSheet, "walk", new[] { 6, 6, 4, 7, 7, 4 });
            AddAnimationCycle(spriteSheet, "walkDown", new[] { 2, 2, 0, 3, 3, 0 });
            AddAnimationCycle(spriteSheet, "walkUp", new[] { 10, 10, 8, 11, 11, 8 });
            AddAnimationCycle(spriteSheet, "cast", new[] { 14, 15});
            AddAnimationCycle(spriteSheet, "castDown", new[] { 12, 13 });
            AddAnimationCycle(spriteSheet, "castUp", new[] { 16, 17 });
            AddAnimationCycle(spriteSheet, "fishing", new[] { 20, 21 });
            AddAnimationCycle(spriteSheet, "fishingDown", new[] { 18, 19 });
            AddAnimationCycle(spriteSheet, "fishingUp", new[] { 22, 23 });
            entity.Attach(new AnimatedSprite(spriteSheet, "idle"));
            entity.Attach(transform);
            entity.Attach(body);
            entity.Attach(player);
            entity.Attach(inventory);
            entity.Attach(new CameraBlock(transform.Position, new Vector2(480, 270)));
            entity.Attach(new Offset { offset = new Vector2(16, 8) });
            entity.Attach(new OnMap());
            if (isLocalPlayer)
                LocalPlayerEntity = entity;

            return entity;
        }

        public Entity CreateNPC(Vector2 position)
        {
            var entity = _world.CreateEntity();
            return entity;
        }

        private void AddAnimationCycle(SpriteSheet spriteSheet, string name, int[] frames, bool isLooping = true, float frameDuration = 0.1f)
        {
            spriteSheet.DefineAnimation(name, builder =>
            {
                builder.IsLooping(isLooping);
                for (int i = 0; i < frames.Length; i++)
                {
                    builder.AddFrame(frames[i], TimeSpan.FromSeconds(frameDuration));
                }
            });
            //var cycle = new SpriteSheetAnimationCycle();
            //foreach (var f in frames)
            //{
            //    cycle.Frames.Add(new SpriteSheetAnimationFrame(f, frameDuration));
            //}

            //cycle.IsLooping = isLooping;

            //spriteSheet.Cycles.Add(name, cycle);
        }

        public void CreateWall(Vector2 position, Vector2 size)
        {
            
            var entity = _world.CreateEntity();
            entity.Attach(new Wall());
            entity.Attach(new Body { Position = position, Size = size, BodyType = BodyType.Static });
        }

        public void CreateWaterArea(Vector2 position, Vector2 size, int depth)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new WaterArea(position, size, depth));
        }


        //Inventory
        public Entity CreateBox(Item item, InventoryComponent inventory)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Box());
            entity.Attach(item);
            entity.Attach(inventory);
            return entity;
        }
        //Placed
        public Entity CreateBox(Item item, InventoryComponent inventory, Transform2 transform, Body body)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new Box());
            entity.Attach(item);
            entity.Attach(inventory);
            entity.Attach(transform);
            entity.Attach(body);
            entity.Attach(new OnMap());
            return entity;
        }

        public Entity CreateFish(Item item, Fish fish)
        {
            var entity = _world.CreateEntity();
            entity.Attach(fish);
            entity.Attach(item);
            return entity;
        }
        public Entity CreateFishingRod(Item item, FishingRod rod)
        {
            var entity = _world.CreateEntity();
            entity.Attach(rod);
            entity.Attach(item);
            return entity;
        }

        public Entity CreateBed(Vector2 position, Vector2 size)
        {
            
            Texture2D bedTex = _contentManager.Load<Texture2D>("Maps/Decorations/bed");
            Texture2DAtlas bedAtlas = Texture2DAtlas.Create("Atlas/Beds", bedTex, 32, 48);
            Sprite bedSprite = bedAtlas.CreateSprite(regionIndex: 0);
            var entity = _world.CreateEntity();
            var bed = new Bed { Position = position, Size = size };
            entity.Attach(bed);
            entity.Attach(bedSprite);
            entity.Attach(new Transform2(position, 0, Vector2.One));
            entity.Attach(new Offset { offset = new Vector2(16, 24) });
            entity.Attach(new OnMap());
            _gameWorld._beds.Add(bed);
            return entity;
        } 

    }
}
