using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

namespace FinLeafIsle.Collisions
{
    public struct AABB
    {
        public AABB(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public Vector2 Min;
        public Vector2 Max;
        public Vector2 Center => new Vector2(CenterX, CenterY);
        public float CenterX => (Max.X - Min.X) / 2f;
        public float CenterY => (Max.Y - Min.Y) / 2f;
        public float Width => Max.X - Min.X;
        public float Height => Max.Y - Min.Y;

        public bool Contain(Vector2 target)
        {
            if (target.X >= Min.X &&
                target.X <= Max.X &&
                target.Y >= Min.Y &&
                target.Y <= Max.Y)
            {
                return true;
            }
            
            return false;
        }
        public bool Contain(Point target)
        {
            if (target.X >= Min.X &&
                    target.X <= Max.X &&
                    target.Y >= Min.Y &&
                    target.Y <= Max.Y)
            {
                return true;
            }
            return false;
        }
    }
}
