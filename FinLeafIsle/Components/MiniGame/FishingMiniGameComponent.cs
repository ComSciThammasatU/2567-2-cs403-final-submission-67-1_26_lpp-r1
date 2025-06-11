using Microsoft.Xna.Framework;

namespace FinLeafIsle.Components.MiniGame
{
    public class FishingMiniGameComponent
    {
        public Vector2 Center { get; set; }
        public float BigCircleRadius { get; set; }
        public float SmallCircleRadius { get; set; }
        public float Progress { get; set; } = 0f;
        public float ProgressSpeed { get; set; } = 4f;
        public float DecaySpeed { get; set; } = 4f;

        public float LastDistanceToCenter { get; set; }

    }
}
