
namespace FinLeafIsle.Components
{
    public enum Facing
    {
        Left, Right, Up, Down
    }
    public enum State
    {
        Idle,
        Walking,  
        Fishing,
    }

    public class Player
    {
        public Facing Facing { get; set; } = Facing.Down;
        public State State { get; set; }
        public bool IsFishing => State == State.Fishing;



    }
}
