using Microsoft.Xna.Framework;

namespace FinLeafIsle
{
    public enum Location
    {
        Town = 0,
        Home = 1,
        Tent = 2,
    }

    public class MapLocation
    {
        public Location Location { get; set; }
        public Vector2 Target { get; set; }
    }
}
