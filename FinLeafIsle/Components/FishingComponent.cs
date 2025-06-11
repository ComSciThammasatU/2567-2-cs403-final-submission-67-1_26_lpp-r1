using FinLeafIsle.Components.ItemComponent;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;

namespace FinLeafIsle.Components
{
    public enum FishingState
    {
        None,
        Charging,
        WaitingForFish,
        FishOnHook,
        Minigame,
        MovingHook,
        CaughtFish,
        CancelFishing,
        GetFish
    }

    public class FishingComponent
    {
        public FishingState State = FishingState.None;
        public float Timer;
        public Vector2 HookCurrentPosition;
        public Vector2 HookStartPosition;
        public Vector2 HookTargetPosition;
        public float HookTravelProgress;
        public float CastDistance = 50;
        public float FishingProgress = 0f;
        public float FishBiteDelay;
        public Entity FishOnHook;
        public bool HasFishOnHook = false; // If a fish is caught
    }
}
