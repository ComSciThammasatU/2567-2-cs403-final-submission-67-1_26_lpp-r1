using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input;
using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using Autofac;
using FinLeafIsle.Components.ItemComponent;
using FinLeafIsle.Components.Inventory;
using Microsoft.Xna.Framework.Audio;


namespace FinLeafIsle.Systems
{
    public class PlayerSystem : EntityProcessingSystem
    {
       
        private ComponentMapper<Player> _playerMapper;
        private ComponentMapper<AnimatedSprite> _spriteMapper;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Body> _bodyMapper;
        private ComponentMapper<FishingComponent> _fishingMapper;
        private GameState _gameState;
        private InventorySlot _handSlot;
        private AudioManager _audioManager;
        private SoundEffectInstance _stepSound;

        

        public PlayerSystem(IContainer container)
            : base(Aspect.All(typeof(Body), typeof(Player), typeof(Transform2), typeof(AnimatedSprite)))
        {
            
            _gameState = container.Resolve<GameState>();
            _handSlot = container.ResolveNamed<InventorySlot>("HandSlot");
            _audioManager = container.Resolve<AudioManager>();
            
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _playerMapper = mapperService.GetMapper<Player>();
            _spriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bodyMapper = mapperService.GetMapper<Body>();
            _fishingMapper = mapperService.GetMapper<FishingComponent>();

            _stepSound = _audioManager.StepingSound.CreateInstance();
            _stepSound.IsLooped = true;
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var keyboardState = KeyboardExtended.GetState();
            var mouseState = MouseExtended.GetState();

            var player = _playerMapper.Get(entityId);
            var sprite = _spriteMapper.Get(entityId);
            var transform = _transformMapper.Get(entityId);
            var body = _bodyMapper.Get(entityId);

            var fishing = new FishingComponent();

            if (_fishingMapper.Has(entityId))
                fishing = _fishingMapper.Get(entityId);

            if (_gameState.State == GState.GamePlay)
            {
               

                //PlayerMovement:
                Vector2 velocity = Vector2.Zero;
                float speed = 120.5f;

                if (!player.IsFishing)
                {
                    if (keyboardState.IsKeyDown(Keys.W))
                    {
                        velocity.Y -= 1;
                        player.Facing = Facing.Up;
                    }
                    else if (keyboardState.IsKeyDown(Keys.S))
                    {
                        velocity.Y += 1;
                        player.Facing = Facing.Down;
                    }
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        velocity.X += 1;
                        player.Facing = Facing.Right;
                    }
                    else if (keyboardState.IsKeyDown(Keys.A))
                    {
                        velocity.X -= 1;
                        player.Facing = Facing.Left;
                    }

                    // Normalize to prevent faster diagonal movement
                    if (velocity != Vector2.Zero)
                    {
                        velocity.Normalize();
                        velocity *= speed;
                    }

                }
                body.Velocity = velocity; // Only one direction at a time

                // Player state logic remains the same
                if (!player.IsFishing)
                {
                    if (velocity != Vector2.Zero)
                        player.State = State.Walking;

                    else
                        player.State = State.Idle;

                }

                if (_handSlot._item != null && _handSlot._item.Has<FishingRod>())
                {
                    if (mouseState.WasButtonPressed(MouseButton.Left) && !_fishingMapper.Has(entityId))
                    {
                        _fishingMapper.Put(entityId, new FishingComponent());
                    }
                }

                

                //PlayerState
                switch (player.State)
                {
                    case State.Walking:
                        if (player.Facing == Facing.Right)
                        {
                            if (sprite.CurrentAnimation != "walk")
                                sprite.SetAnimation("walk");

                            sprite.Effect = SpriteEffects.None;
                        }
                        else if (player.Facing == Facing.Left)
                        {
                            if (sprite.CurrentAnimation != "walk")
                                sprite.SetAnimation("walk");

                            sprite.Effect = SpriteEffects.FlipHorizontally;
                        }
                        else if (player.Facing == Facing.Up)
                        {
                            if (sprite.CurrentAnimation != "walkUp")
                                sprite.SetAnimation("walkUp");
                        }
                        else if (player.Facing == Facing.Down)
                        {
                            if (sprite.CurrentAnimation != "walkDown")
                                sprite.SetAnimation("walkDown");
                        }


                        break;

                    case State.Idle:
                        if (player.Facing == Facing.Right)
                        {
                            if (sprite.CurrentAnimation != "idle")
                                sprite.SetAnimation("idle");

                            sprite.Effect = SpriteEffects.None;
                        }
                        else if (player.Facing == Facing.Left)
                        {
                            if (sprite.CurrentAnimation != "idle")
                                sprite.SetAnimation("idle");

                            sprite.Effect = SpriteEffects.FlipHorizontally;
                        }
                        else if (player.Facing == Facing.Up)
                        {
                            if (sprite.CurrentAnimation != "idleUp")
                                sprite.SetAnimation("idleUp");
                        }
                        else if (player.Facing == Facing.Down)
                        {
                            if (sprite.CurrentAnimation != "idleDown")
                                sprite.SetAnimation("idleDown");
                        }

                        break;

                    case State.Fishing:

                        if (fishing.State == FishingState.Charging)
                        {
                            if (player.Facing == Facing.Right)
                            {
                                if (sprite.CurrentAnimation != "cast")
                                    sprite.SetAnimation("cast");

                                sprite.Effect = SpriteEffects.None;
                            }
                            else if (player.Facing == Facing.Left)
                            {
                                if (sprite.CurrentAnimation != "cast")
                                    sprite.SetAnimation("cast");

                                sprite.Effect = SpriteEffects.FlipHorizontally;
                            }
                            else if (player.Facing == Facing.Up)
                            {
                                if (sprite.CurrentAnimation != "castUp")
                                    sprite.SetAnimation("castUp");
                            }
                            else if (player.Facing == Facing.Down)
                            {
                                if (sprite.CurrentAnimation != "castDown")
                                    sprite.SetAnimation("castDown");
                            }
                        }
                        else
                        {
                            if (player.Facing == Facing.Right)
                            {
                                if (sprite.CurrentAnimation != "fishing")
                                    sprite.SetAnimation("fishing");

                                sprite.Effect = SpriteEffects.None;
                            }
                            else if (player.Facing == Facing.Left)
                            {
                                if (sprite.CurrentAnimation != "fishing")
                                    sprite.SetAnimation("fishing");

                                sprite.Effect = SpriteEffects.FlipHorizontally;
                            }
                            else if (player.Facing == Facing.Up)
                            {
                                if (sprite.CurrentAnimation != "fishingUp")
                                    sprite.SetAnimation("fishingUp");
                            }
                            else if (player.Facing == Facing.Down)
                            {
                                if (sprite.CurrentAnimation != "fishingDown")
                                    sprite.SetAnimation("fishingDown");
                            }
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();


                }

                //body.Velocity *= 0.7f;


            }
            else
            {
                player.State = State.Idle;
            }

            if (player.State == State.Walking)
                _stepSound.Play();
            else
                _stepSound.Stop();


        }

    }
}
